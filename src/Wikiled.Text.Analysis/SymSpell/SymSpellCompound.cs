using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wikiled.Text.Analysis.SymSpell
{
    public class SymSpellCompound : ISymSpellCompound
    {
        //0: top suggestion
        //1: all suggestions of smallest edit distance   
        //2: all suggestions <= editDistanceMax (slower, no early termination)

        //Dictionary that contains both the original words and the deletes derived from them. A term might be both word and delete from another word at the same time.
        //For space reduction a item might be either of type dictionaryItem or Int. 
        //A dictionaryItem is used for word, word/delete, and delete with multiple suggestions. Int is used for deletes with a single suggestion (the majority of entries).
        //A Dictionary with fixed value type (int) requires less memory than a Dictionary with variable value type (object)
        //To support two types with a Dictionary with fixed type (int), positive number point to one list of type 1 (string), and negative numbers point to a secondary list of type 2 (dictionaryEntry)
        private readonly Dictionary<string, int> dictionary = new Dictionary<string, int>(); //initialisation

        private readonly List<DictionaryItem> itemlist = new List<DictionaryItem>();

        //List of unique words. By using the suggestions (Int) as index for this list they are translated into the original string.
        private readonly List<string> wordlist = new List<string>();

        //false: assumes input string as single term, no compound splitting / decompounding
        //true:  supports compound splitting / decompounding with three cases:
        //1. mistakenly inserted space into a correct word led to two incorrect terms 
        //2. mistakenly omitted space between two correct words led to one incorrect combined term
        //3. multiple independent input terms with/without spelling errors

        private readonly int editDistanceMax = 2;

        private int maxlength; //maximum dictionary term length

        private readonly int verbose = 0; //ALLWAYS use verbose = 0 if enableCompoundCheck = true!

        //for every word there all deletes with an edit distance of 1..editDistanceMax created and added to the dictionary
        //every delete entry has a suggestions list, which points to the original term(s) it was created from
        //The dictionary may be dynamically updated (word frequency and new words) at any time by calling createDictionaryEntry
        public bool CreateDictionaryEntry(string key, long count)
        {
            //a treshold might be specifid, when a term occurs so frequently in the corpus that it is considered a valid word for spelling correction
            int countTreshold = 1;
            long countPrevious = 0;
            bool result = false;
            DictionaryItem value = null;
            int valueo;
            if (dictionary.TryGetValue(key, out valueo))
            {
                //new word, but identical single delete existed before
                //+ = single delete = index auf worlist 
                //- = !single delete (word / word + delete(s) / deletes) = index to dictionaryItem list
                if (valueo >= 0)
                {
                    int tmp = valueo;
                    value = new DictionaryItem();
                    value.Suggestions.Add(tmp);
                    value.Suggestions.TrimExcess();
                    itemlist.Add(value);
                    dictionary[key] = -itemlist.Count;
                }

                //existing word (word appears several times)
                else
                {
                    value = itemlist[-valueo - 1];
                }

                countPrevious = value.Count;

                //summarizes multiple frequency entries of a word (prevents overflow)
                value.Count = Math.Min(long.MaxValue, value.Count + count);
            }
            else
            {
                //new word
                value = new DictionaryItem();
                value.Count = count;
                itemlist.Add(value);
                dictionary[key] = -itemlist.Count;

                if (key.Length > maxlength)
                {
                    maxlength = key.Length;
                }
            }

            //edits/suggestions are created only once, no matter how often word occurs
            //edits/suggestions are created only as soon as the word occurs in the corpus, 
            //even if the same term existed before in the dictionary as an edit from another word
            if (value.Count >= countTreshold &&
                countPrevious < countTreshold)
            {
                //word2index
                wordlist.Add(key);
                int keyint = wordlist.Count - 1;

                result = true;

                //create deletes
                foreach (string delete in Edits(key, 0, new HashSet<string>()))
                {
                    int value2;
                    DictionaryItem di;
                    if (dictionary.TryGetValue(delete, out value2))
                    {
                        //already exists:
                        //1. word1==deletes(word2) 
                        //2. deletes(word1)==deletes(word2) 
                        //int or dictionaryItem? single delete existed before!
                        if (value2 >= 0)
                        {
                            //transformes int to dictionaryItem
                            di = new DictionaryItem();
                            di.Suggestions.Add(value2);
                            di.Suggestions.TrimExcess();
                            itemlist.Add(di);
                            dictionary[delete] = -itemlist.Count;
                            if (!di.Suggestions.Contains(keyint))
                            {
                                AddLowestDistance(di, key, keyint, delete);
                            }
                        }
                        else
                        {
                            di = itemlist[-value2 - 1];
                            if (!di.Suggestions.Contains(keyint))
                            {
                                AddLowestDistance(di, key, keyint, delete);
                            }
                        }
                    }
                    else
                    {
                        dictionary.Add(delete, keyint);
                    }
                }
            }
            return result;
        }

        public List<SuggestItem> Lookup(string input, int editDistanceMax)
        {
            input = input.ToLower().Trim();
            //save some time
            if (input.Length - editDistanceMax > maxlength)
            {
                return new List<SuggestItem>();
            }

            List<string> candidates = new List<string>();
            HashSet<string> hashset1 = new HashSet<string>();

            List<SuggestItem> suggestions = new List<SuggestItem>();
            HashSet<string> hashset2 = new HashSet<string>();

            int valueo;

            //add original term
            candidates.Add(input);

            while (candidates.Count > 0)
            {
                string candidate = candidates[0];
                candidates.RemoveAt(0);

                //save some time
                //early termination
                //suggestion distance=candidate.distance... candidate.distance+editDistanceMax                
                //if canddate distance is already higher than suggestion distance, than there are no better suggestions to be expected
                if (verbose < 2 &&
                    suggestions.Count > 0 &&
                    input.Length - candidate.Length > suggestions[0].Distance)
                {
                    return ReturnSorted(suggestions);
                }

                //read candidate entry from dictionary
                if (dictionary.TryGetValue(candidate, out valueo))
                {
                    DictionaryItem value = new DictionaryItem();
                    if (valueo >= 0)
                    {
                        value.Suggestions.Add(valueo);
                    }
                    else
                    {
                        value = itemlist[-valueo - 1];
                    }

                    //if count>0 then candidate entry is correct dictionary term, not only delete item
                    if (value.Count > 0 &&
                        hashset2.Add(candidate))
                    {
                        int distance = input.Length - candidate.Length;

                        //save some time
                        //do not process higher distances than those already found, if verbose<2      
                        if (verbose == 2 ||
                            suggestions.Count == 0 ||
                            distance <= suggestions[0].Distance)
                        {
                            //Fix: previously not allways all suggestons within editdistance (verbose=1) or the best suggestion (verbose=0) were returned : e.g. elove did not return love
                            //suggestions.Clear() was not executed in this branch, if a suggestion with lower edit distance was added here (for verbose<2). 
                            //Then possibly suggestions with higher edit distance remained on top, the suggestion with lower edit distance were added to the end. 
                            //All of them where deleted later once a suggestion with a lower distance than the first item in the list was later added in the other branch. 
                            //Therefore returned suggestions were not always complete for verbose<2.
                            //remove all existing suggestions of higher distance, if verbose<2
                            if (verbose < 2 &&
                                suggestions.Count > 0 &&
                                suggestions[0].Distance > distance)
                            {
                                suggestions.Clear();
                            }

                            //add correct dictionary term term to suggestion list
                            SuggestItem si = new SuggestItem(candidate, value.Count, distance);
                            suggestions.Add(si);

                            //early termination
                            if (verbose < 2 &&
                                input.Length - candidate.Length == 0)
                            {
                                return ReturnSorted(suggestions);
                            }
                        }
                    }

                    //iterate through suggestions (to other correct dictionary items) of delete item and add them to suggestion list
                    int value2;
                    foreach (int suggestionint in value.Suggestions)
                    {
                        //save some time 
                        //skipping double items early: different deletes of the input term can lead to the same suggestion
                        //index2word
                        string suggestion = wordlist[suggestionint];
                        if (hashset2.Add(suggestion))
                        {
                            //True Damerau-Levenshtein Edit Distance: adjust distance, if both distances>0
                            //We allow simultaneous edits (deletes) of editDistanceMax on on both the dictionary and the input term. 
                            //For replaces and adjacent transposes the resulting edit distance stays <= editDistanceMax.
                            //For inserts and deletes the resulting edit distance might exceed editDistanceMax.
                            //To prevent suggestions of a higher edit distance, we need to calculate the resulting edit distance, if there are simultaneous edits on both sides.
                            //Example: (bank==bnak and bank==bink, but bank!=kanb and bank!=xban and bank!=baxn for editDistanceMaxe=1)
                            //Two deletes on each side of a pair makes them all equal, but the first two pairs have edit distance=1, the others edit distance=2.
                            int distance = 0;
                            if (suggestion != input)
                            {
                                if (suggestion.Length == candidate.Length)
                                {
                                    distance = input.Length - candidate.Length;
                                }
                                else if (input.Length == candidate.Length)
                                {
                                    distance = suggestion.Length - candidate.Length;
                                }
                                else
                                {
                                    //common prefixes and suffixes are ignored, because this speeds up the Damerau-levenshtein-Distance calculation without changing it.
                                    int ii = 0;
                                    int jj = 0;
                                    while (ii < suggestion.Length &&
                                           ii < input.Length &&
                                           suggestion[ii] == input[ii])
                                    {
                                        ii++;
                                    }

                                    while (jj < suggestion.Length - ii &&
                                           jj < input.Length - ii &&
                                           suggestion[suggestion.Length - jj - 1] == input[input.Length - jj - 1])
                                    {
                                        jj++;
                                    }

                                    if (ii > 0 ||
                                        jj > 0)
                                    {
                                        distance = suggestion.Substring(ii, suggestion.Length - ii - jj).DamerauLevenshteinDistance2(input.Substring(ii, input.Length - ii - jj));
                                    }
                                    else
                                    {
                                        distance = suggestion.DamerauLevenshteinDistance2(input);
                                    }
                                }
                            }

                            //save some time
                            //do not process higher distances than those already found, if verbose<2
                            if (verbose < 2 &&
                                suggestions.Count > 0 &&
                                distance > suggestions[0].Distance)
                            {
                                continue;
                            }

                            if (distance <= editDistanceMax)
                            {
                                if (dictionary.TryGetValue(suggestion, out value2))
                                {
                                    SuggestItem si = new SuggestItem(suggestion, itemlist[-value2 - 1].Count, distance);

                                    //remove all existing suggestions of higher distance, if verbose<2
                                    if (verbose < 2 &&
                                        suggestions.Count > 0 &&
                                        suggestions[0].Distance > distance)
                                    {
                                        suggestions.Clear();
                                    }

                                    suggestions.Add(si);
                                }
                            }
                        }
                    } //end foreach
                } //end if         

                //add edits 
                //derive edits (deletes) from candidate (input) and add them to candidates list
                //this is a recursive process until the maximum edit distance has been reached
                if (input.Length - candidate.Length < editDistanceMax)
                {
                    //save some time
                    //do not create edits with edit distance smaller than suggestions already found
                    if (verbose < 2 &&
                        suggestions.Count > 0 &&
                        input.Length - candidate.Length >= suggestions[0].Distance)
                    {
                        continue;
                    }

                    for (int i = 0; i < candidate.Length; i++)
                    {
                        string delete = candidate.Remove(i, 1);
                        if (hashset1.Add(delete))
                        {
                            candidates.Add(delete);
                        }
                    }
                }
            } //end while

            //sort by ascending edit distance, then by descending word frequency
            return ReturnSorted(suggestions);
        }

        public List<SuggestItem> LookupCompound(string input, int editDistanceMax)
        {
            input = input.ToLower().Trim();
            //parse input string into single terms
            string[] termList1 = ParseWords(input).ToArray();

            List<SuggestItem> suggestionsPreviousTerm; //suggestions for a single term
            List<SuggestItem> suggestions = new List<SuggestItem>(); //suggestions for a single term
            List<SuggestItem> suggestionParts = new List<SuggestItem>(); //1 line with separate parts

            //translate every term to its best suggestion, otherwise it remains unchanged
            bool lastCombi = false;
            for (int i = 0; i < termList1.Length; i++)
            {
                suggestionsPreviousTerm = new List<SuggestItem>(suggestions.Count);
                for (int k = 0; k < suggestions.Count; k++)
                {
                    suggestionsPreviousTerm.Add(suggestions[k].ShallowCopy());
                }

                suggestions = Lookup(termList1[i], editDistanceMax);

                //combi check, always before split
                if (i > 0 &&
                    !lastCombi)
                {
                    List<SuggestItem> suggestionsCombi = Lookup(termList1[i - 1] + termList1[i], editDistanceMax);

                    if (suggestionsCombi.Count > 0)
                    {
                        SuggestItem best1 = suggestionParts[suggestionParts.Count - 1];
                        SuggestItem best2;
                        if (suggestions.Count > 0)
                        {
                            best2 = suggestions[0];
                        }
                        else
                        {
                            best2 = new SuggestItem(termList1[i], editDistanceMax + 1, 0);
                        }

                        if (suggestionsCombi[0].Distance + 1 < (termList1[i - 1] + " " + termList1[i]).DamerauLevenshteinDistance2(best1.Term + " " + best2.Term))
                        {
                            suggestionsCombi[0].IncreaseDistance();
                            suggestionParts[suggestionParts.Count - 1] = suggestionsCombi[0];
                            break;
                        }
                    }
                }

                //alway split terms without suggestion / never split terms with suggestion ed=0 / never split single char terms
                if (suggestions.Count > 0 &&
                    (suggestions[0].Distance == 0 || termList1[i].Length == 1))
                {
                    //choose best suggestion
                    suggestionParts.Add(suggestions[0]);
                }
                else
                {
                    //if no perfect suggestion, split word into pairs
                    List<SuggestItem> suggestionsSplit = new List<SuggestItem>();

                    //add original term
                    if (suggestions.Count > 0)
                    {
                        suggestionsSplit.Add(suggestions[0]);
                    }

                    if (termList1[i].Length > 1)
                    {
                        for (int j = 1; j < termList1[i].Length; j++)
                        {
                            string part1 = termList1[i].Substring(0, j);
                            string part2 = termList1[i].Substring(j);

                            List<SuggestItem> suggestions1 = Lookup(part1, editDistanceMax);
                            if (suggestions1.Count > 0)
                            {
                                if (suggestions.Count > 0 &&
                                    suggestions[0].Term == suggestions1[0].Term)
                                {
                                    break;
                                }

                                //if split correction1 == einzelwort correction
                                List<SuggestItem> suggestions2 = Lookup(part2, editDistanceMax);
                                if (suggestions2.Count > 0)
                                {
                                    if (suggestions.Count > 0 &&
                                        suggestions[0].Term == suggestions2[0].Term)
                                    {
                                        break;
                                    }

                                    //if split correction1 == einzelwort correction
                                    //select best suggestion for split pair
                                    var suggestionSplitTerm = suggestions1[0].Term + " " + suggestions2[0].Term;
                                    var suggestionSplitDistance = termList1[i].DamerauLevenshteinDistance2(suggestions1[0].Term + " " + suggestions2[0].Term);
                                    var suggestionSplitCount = Math.Min(suggestions1[0].Count, suggestions2[0].Count);
                                    SuggestItem suggestionSplit = new SuggestItem(suggestionSplitTerm, suggestionSplitCount, suggestionSplitDistance);
                                    suggestionsSplit.Add(suggestionSplit);

                                    //early termination of split
                                    if (suggestionSplit.Distance == 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (suggestionsSplit.Count > 0)
                        {
                            //select best suggestion for split pair
                            suggestionsSplit.Sort((x, y) => 2 * x.Distance.CompareTo(y.Distance) - x.Count.CompareTo(y.Count));
                            suggestionParts.Add(suggestionsSplit[0]);
                        }
                        else
                        {
                            SuggestItem si = new SuggestItem(termList1[i], 0, editDistanceMax + 1);
                            suggestionParts.Add(si);
                        }
                    }
                    else
                    {
                        SuggestItem si = new SuggestItem(termList1[i], 0, editDistanceMax + 1);
                        suggestionParts.Add(si);
                    }
                }
            }

            var suggestionCount = long.MaxValue;
            string s = "";
            foreach (SuggestItem si in suggestionParts)
            {
                s += si.Term + " ";
                suggestionCount = Math.Min(suggestionCount, si.Count);
            }

            var suggestionTerm = s.TrimEnd();
            var suggestionDistance = suggestionTerm.DamerauLevenshteinDistance2(input);
            var suggestion = new SuggestItem(suggestionTerm, suggestionCount, suggestionDistance);
            List<SuggestItem> suggestionsLine = new List<SuggestItem>();
            suggestionsLine.Add(suggestion);
            return suggestionsLine;
        }

        //save some time and space
        private void AddLowestDistance(DictionaryItem item, string suggestion, int suggestionint, string delete)
        {
            //remove all existing suggestions of higher distance, if verbose<2
            //index2word
            if (verbose < 2 &&
                item.Suggestions.Count > 0 &&
                wordlist[item.Suggestions[0]].Length - delete.Length > suggestion.Length - delete.Length)
            {
                item.Suggestions.Clear();
            }

            //do not add suggestion of higher distance than existing, if verbose<2
            if (verbose == 2 ||
                item.Suggestions.Count == 0 ||
                wordlist[item.Suggestions[0]].Length - delete.Length >= suggestion.Length - delete.Length)
            {
                item.Suggestions.Add(suggestionint);
                item.Suggestions.TrimExcess();
            }
        }

        //inexpensive and language independent: only deletes, no transposes + replaces + inserts
        //replaces and inserts are expensive and language dependent (Chinese has 70,000 Unicode Han characters)
        private HashSet<string> Edits(string word, int editDistance, HashSet<string> deletes)
        {
            editDistance++;
            if (word.Length > 1)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    string delete = word.Remove(i, 1);
                    if (deletes.Add(delete))
                    {
                        //recursion, if maximum edit distance not yet reached
                        if (editDistance < editDistanceMax)
                        {
                            Edits(delete, editDistance, deletes);
                        }
                    }
                }
            }

            return deletes;
        }

        //create a non-unique wordlist from sample text
        //language independent (e.g. works with Chinese characters)
        private IEnumerable<string> ParseWords(string text)
        {
            // \w Alphanumeric characters (including non-latin characters, umlaut characters and digits) plus "_" 
            // \d Digits
            // Compatible with non-latin characters, does not split words at apostrophes
            return Regex.Matches(text.ToLower(), @"['’\w-[_]]+").Cast<Match>().Select(m => m.Value);

            //for benchmarking only: with CreateDictionary("big.txt","") and the text corpus from http://norvig.com/big.txt  the Regex below provides the exact same number of dictionary items as Norvigs regex "[a-z]+" (which splits words at apostrophes & incompatible with non-latin characters)
            //return Regex.Matches(text.ToLower(), @"[\w-[\d_]]+").Cast<Match>().Select(m => m.Value);        
        }

        private List<SuggestItem> ReturnSorted(List<SuggestItem> suggestions)
        {
            if (verbose < 2)
            {
                suggestions.Sort((x, y) => -x.Count.CompareTo(y.Count));
            }
            else
            {
                suggestions.Sort((x, y) => 2 * x.Distance.CompareTo(y.Distance) - x.Count.CompareTo(y.Count));
            }

            if (verbose == 0 && suggestions.Count > 1)
            {
                return suggestions.GetRange(0, 1);
            }

            return suggestions;
        }
    }
}
