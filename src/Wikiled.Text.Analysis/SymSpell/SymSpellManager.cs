using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Wikiled.Text.Analysis.SymSpell
{
    public class SymSpellManager : ISymSpell
    {
        //0: top suggestion
        //1: all suggestions of smallest edit distance   
        //2: all suggestions <= editDistanceMax (slower, no early termination)

        //Dictionary that contains both the original words and the deletes derived from them. A term might be both word and delete from another word at the same time.
        //For space reduction a item might be either of type dictionaryItem or Int. 
        //A dictionaryItem is used for word, word/delete, and delete with multiple suggestions. Int is used for deletes with a single suggestion (the majority of entries).
        //A Dictionary with fixed value type (int) requires less memory than a Dictionary with variable value type (object)
        //To support two types with a Dictionary with fixed type (int), positive number point to one list of type 1 (string), and negative numbers point to a secondary list of type 2 (dictionaryEntry)
        private readonly Dictionary<string, int> dictionary = new Dictionary<string, int>(); //initialisierung

        private readonly List<DictionaryItem> itemlist = new List<DictionaryItem>();

        //List of unique words. By using the suggestions (Int) as index for this list they are translated into the original string.
        private readonly List<string> wordlist = new List<string>();

        private readonly int editDistanceMax = 2;

        private readonly int lp = 7; //prefix length  5..7

        private int maxlength; //maximum dictionary term length

        private readonly int verbose = 0;


        //for every word there all deletes with an edit distance of 1..editDistanceMax created and added to the dictionary
        //every delete entry has a suggestions list, which points to the original term(s) it was created from
        //The dictionary may be dynamically updated (word frequency and new words) at any time by calling createDictionaryEntry
        public bool AddRecord(string key, string language, long count)
        {
            //a treshold might be specifid, when a term occurs so frequently in the corpus that it is considered a valid word for spelling correction
            int countTreshold = 1;
            long countPrevious = 0;
            bool result = false;
            DictionaryItem value = null;

            //Int32 valueo;
            if (dictionary.TryGetValue(language + key, out int valueo))
            {
                //new word, but identical single delete existed before
                //+ = single delete = index auf worlist 
                //- = !single delete (word / word + delete(s) / deletes) = index to dictionaryItem list
                if (valueo >= 0)
                {
                    int tmp = valueo;
                    value = new DictionaryItem();
                    value.Suggestions.Add(tmp);
                    itemlist.Add(value);
                    dictionary[language + key] = -itemlist.Count;
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
                value = new DictionaryItem
                        {
                            Count = count
                        };

                itemlist.Add(value);
                dictionary[language + key] = -itemlist.Count;

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
                foreach (string delete in EditsPrefix(key))
                {
                    //Int32 value2;
                    DictionaryItem di;
                    if (dictionary.TryGetValue(language + delete, out int value2))
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
                            itemlist.Add(di);
                            dictionary[language + delete] = -itemlist.Count;
                            if (!di.Suggestions.Contains(keyint))
                            {
                                di.Suggestions.Add(keyint);
                            }
                        }
                        else
                        {
                            di = itemlist[-value2 - 1];
                            if (!di.Suggestions.Contains(keyint))
                            {
                                di.Suggestions.Add(keyint);
                            }
                        }
                    }
                    else
                    {
                        dictionary.Add(language + delete, keyint);
                    }
                }
            }

            return result;
        }

        public HashSet<string> EditsPrefix(string key)
        {
            HashSet<string> hashSet = new HashSet<string>();
            if (key.Length <= editDistanceMax)
            {
                hashSet.Add(""); //Fix: add ""-delete
            }

            return Edits(key.Length <= lp ? key : key.Substring(0, lp), 0, hashSet);
        }

        //load a frequency dictionary
     
        public List<SuggestItem> Lookup(string input, string language, int editDistanceMax)
        {
            //save some time
            if (input.Length - editDistanceMax > maxlength)
            {
                return new List<SuggestItem>();
            }

            List<string> candidates = new List<string>();
            HashSet<string> hashset1 = new HashSet<string>();

            List<SuggestItem> suggestions = new List<SuggestItem>();
            HashSet<string> hashset2 = new HashSet<string>();

            int editDistanceMax2 = editDistanceMax;

            int candidatePointer = 0;

            //add original term
            candidates.Add(input);

            while (candidatePointer < candidates.Count)
            {
                string candidate = candidates[candidatePointer++];
                int lengthDiff = Math.Min(input.Length, lp) - candidate.Length;

                //save some time
                //early termination
                //suggestion distance=candidate.distance... candidate.distance+editDistanceMax                
                //if canddate distance is already higher than suggestion distance, than there are no better suggestions to be expected
                if (verbose < 2 &&
                    suggestions.Count > 0 &&
                    lengthDiff > suggestions[0].Distance)
                {
                    return SortItems(suggestions);
                }

                //read candidate entry from dictionary
                if (dictionary.TryGetValue(language + candidate, out int valueo))
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
                    if (value.Count > 0)
                    {
                        int distance = input.Length - candidate.Length;

                        //save some time
                        //do not process higher distances than those already found, if verbose<2      
                        if (distance <= editDistanceMax &&
                            (verbose == 2 || suggestions.Count == 0 || distance <= suggestions[0].Distance) &&
                            hashset2.Add(candidate))
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
                                suggestions.Clear(); //!!!

                            //add correct dictionary term term to suggestion list
                            SuggestItem si = new SuggestItem(
                                candidate,
                                value.Count,
                                distance);
                            suggestions.Add(si);

                            //early termination
                            if (verbose < 2 &&
                                distance == 0)
                            {
                                return SortItems(suggestions);
                            }
                        }
                    }

                    //iterate through suggestions (to other correct dictionary items) of delete item and add them to suggestion list
                    foreach (int suggestionint in value.Suggestions)
                    {
                        //save some time 
                        //skipping double items early: different deletes of the input term can lead to the same suggestion
                        //index2word
                        string suggestion = wordlist[suggestionint];

                        //True Damerau-Levenshtein Edit Distance: adjust distance, if both distances>0
                        //We allow simultaneous edits (deletes) of editDistanceMax on on both the dictionary and the input term. 
                        //For replaces and adjacent transposes the resulting edit distance stays <= editDistanceMax.
                        //For inserts and deletes the resulting edit distance might exceed editDistanceMax.
                        //To prevent suggestions of a higher edit distance, we need to calculate the resulting edit distance, if there are simultaneous edits on both sides.
                        //Example: (bank==bnak and bank==bink, but bank!=kanb and bank!=xban and bank!=baxn for editDistanceMaxe=1)
                        //Two deletes on each side of a pair makes them all equal, but the first two pairs have edit distance=1, the others edit distance=2.
                        int distance = 0; // editDistanceMax+1;
                        if (suggestion != input)
                        {
                            int min = 0;
                            if (Math.Abs(suggestion.Length - input.Length) > editDistanceMax2)
                            {
                                continue;
                            }
                            if (candidate.Length == 0)
                            {
                                //suggestions which have no common chars with input (input.length<=editDistanceMax && suggestion.length<=editDistanceMax)
                                if (!hashset2.Add(suggestion))
                                {
                                    continue;
                                }

                                distance = Math.Max(input.Length, suggestion.Length);
                            }
                            else

                                //number of edits in prefix ==maxediddistance  AND no identic suffix, then editdistance>editdistancemax and no need for Levenshtein calculation  
                                //                                                 (input.Length >= lp) && (suggestion.Length >= lp) 
                            if (lp - editDistanceMax == candidate.Length &&
                                (min = Math.Min(input.Length, suggestion.Length) - lp) > 1 &&
                                input.Substring(input.Length + 1 - min) != suggestion.Substring(suggestion.Length + 1 - min) ||
                                min > 0 &&
                                input[input.Length - min] != suggestion[suggestion.Length - min] &&
                                (input[input.Length - min - 1] != suggestion[suggestion.Length - min] || input[input.Length - min] != suggestion[suggestion.Length - min - 1]))
                            {
                                continue;
                            }
                            else //edit distance of remaining string (after prefix)
                            {
                                if (suggestion.Length == candidate.Length &&
                                    input.Length <= lp)
                                {
                                    if (!hashset2.Add(suggestion))
                                    {
                                        continue;
                                    }

                                    distance = input.Length - candidate.Length;
                                }
                                else if (input.Length == candidate.Length &&
                                         suggestion.Length <= lp)
                                {
                                    if (!hashset2.Add(suggestion))
                                    {
                                        continue;
                                    }

                                    distance = suggestion.Length - candidate.Length;
                                }
                                else if (hashset2.Add(suggestion))
                                {
                                    distance = input.DamerauLevenshteinDistance(suggestion, editDistanceMax2);
                                    if (distance < 0)
                                    {
                                        distance = editDistanceMax + 1;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else if (!hashset2.Add(suggestion))
                        {
                            continue;
                        }

                        //save some time
                        //do not process higher distances than those already found, if verbose<2
                        if (verbose < 2 &&
                            suggestions.Count > 0 &&
                            distance > suggestions[0].Distance)
                            continue;
                        if (distance <= editDistanceMax)
                        {
                            if (dictionary.TryGetValue(language + suggestion, out int value2))
                            {
                                SuggestItem si = new SuggestItem(
                                    suggestion,
                                    itemlist[-value2 - 1].Count,
                                    distance);

                                //we will calculate DamLev distance only to the smallest found distance sof far
                                if (verbose < 2)
                                    editDistanceMax2 = distance;

                                //remove all existing suggestions of higher distance, if verbose<2
                                if (verbose < 2 &&
                                    suggestions.Count > 0 &&
                                    suggestions[0].Distance > distance)
                                    suggestions.Clear();
                                suggestions.Add(si);
                            }
                        }
                    } //end foreach
                } //end if         

                //add edits 
                //derive edits (deletes) from candidate (input) and add them to candidates list
                //this is a recursive process until the maximum edit distance has been reached
                if (lengthDiff < editDistanceMax)
                {
                    //save some time
                    //do not create edits with edit distance smaller than suggestions already found
                    //if ((verbose < 2) && (suggestions.Count > 0) && (input.Length - candidate.Length >= suggestions[0].distance)) continue;
                    if (verbose < 2 &&
                        suggestions.Count > 0 &&
                        lengthDiff >= suggestions[0].Distance)
                        continue; //!?!

                    if (candidate.Length > lp)
                        candidate = candidate.Substring(0, lp); //just the input entry might be > lp
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

            return SortItems(suggestions);
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
                            Edits(delete, editDistance, deletes);
                    }
                }
            }

            return deletes;
        }

        //create a non-unique wordlist from sample text
        //language independent (e.g. works with Chinese characters)
        private string[] ParseWords(string text)
        {
            // \w Alphanumeric characters (including non-latin characters, umlaut characters and digits) plus "_" 
            // \d Digits
            // Compatible with non-latin characters, does not split words at apostrophes
            MatchCollection mc = Regex.Matches(text.ToLower(), @"['’\w-[_]]+");

            //for benchmarking only: with CreateDictionary("big.txt","") and the text corpus from http://norvig.com/big.txt  the Regex below provides the exact same number of dictionary items as Norvigs regex "[a-z]+" (which splits words at apostrophes & incompatible with non-latin characters)     
            //MatchCollection mc = Regex.Matches(text.ToLower(), @"[\w-[\d_]]+");

            var matches = new string[mc.Count];
            for (int i = 0; i < matches.Length; i++)
            {
                matches[i] = mc[i].ToString();
            }

            return matches;
        }

        private List<SuggestItem> SortItems(List<SuggestItem> suggestions)
        {
            if (verbose < 2)
            {
                suggestions.Sort((x, y) => -x.Count.CompareTo(y.Count));
            }
            else
            {
                suggestions.Sort((x, y) => 2 * x.Distance.CompareTo(y.Distance) - x.Count.CompareTo(y.Count));
            }

            if (verbose == 0 &&
                suggestions.Count > 1)
            {
                return suggestions.GetRange(0, 1);
            }

            return suggestions;
        }
    }
}
