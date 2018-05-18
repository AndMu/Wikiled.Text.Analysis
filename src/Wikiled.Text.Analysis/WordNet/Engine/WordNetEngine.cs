using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wikiled.Common.Extensions;
using Wikiled.Text.Analysis.POS;

namespace Wikiled.Text.Analysis.WordNet.Engine
{
    /// <summary>
    /// Provides access to the WordNet resource via two alternative methods, in-memory and disk-based. The latter is blazingly
    /// fast but also hugely inefficient in terms of memory consumption. The latter uses essentially zero memory but is slow
    /// because all searches have to be conducted on-disk.
    /// </summary>
    public class WordNetEngine : IWordNetEngine
    {
        /// <summary>
        /// SynSet relation symbols that are available for each POS
        /// </summary>
        private static readonly Dictionary<WordType, Dictionary<string, SynSetRelation>> posSymbolRelation;

        /// <summary>
        /// Static constructor
        /// </summary>
        static WordNetEngine()
        {
            posSymbolRelation = new Dictionary<WordType, Dictionary<string, SynSetRelation>>();

            // noun relations
            Dictionary<string, SynSetRelation> nounSymbolRelation = new Dictionary<string, SynSetRelation>(StringComparer.OrdinalIgnoreCase);
            nounSymbolRelation.Add("!", SynSetRelation.Antonym);
            nounSymbolRelation.Add("@", SynSetRelation.Hypernym);
            nounSymbolRelation.Add("@i", SynSetRelation.InstanceHypernym);
            nounSymbolRelation.Add("~", SynSetRelation.Hyponym);
            nounSymbolRelation.Add("~i", SynSetRelation.InstanceHyponym);
            nounSymbolRelation.Add("#m", SynSetRelation.MemberHolonym);
            nounSymbolRelation.Add("#s", SynSetRelation.SubstanceHolonym);
            nounSymbolRelation.Add("#p", SynSetRelation.PartHolonym);
            nounSymbolRelation.Add("%m", SynSetRelation.MemberMeronym);
            nounSymbolRelation.Add("%s", SynSetRelation.SubstanceMeronym);
            nounSymbolRelation.Add("%p", SynSetRelation.PartMeronym);
            nounSymbolRelation.Add("=", SynSetRelation.Attribute);
            nounSymbolRelation.Add("+", SynSetRelation.DerivationallyRelated);
            nounSymbolRelation.Add(";c", SynSetRelation.TopicDomain);
            nounSymbolRelation.Add("-c", SynSetRelation.TopicDomainMember);
            nounSymbolRelation.Add(";r", SynSetRelation.RegionDomain);
            nounSymbolRelation.Add("-r", SynSetRelation.RegionDomainMember);
            nounSymbolRelation.Add(";u", SynSetRelation.UsageDomain);
            nounSymbolRelation.Add("-u", SynSetRelation.UsageDomainMember);
            posSymbolRelation.Add(WordType.Noun, nounSymbolRelation);

            // verb relations
            Dictionary<string, SynSetRelation> verbSymbolRelation = new Dictionary<string, SynSetRelation>(StringComparer.OrdinalIgnoreCase);
            verbSymbolRelation.Add("!", SynSetRelation.Antonym);
            verbSymbolRelation.Add("@", SynSetRelation.Hypernym);
            verbSymbolRelation.Add("~", SynSetRelation.Hyponym);
            verbSymbolRelation.Add("*", SynSetRelation.Entailment);
            verbSymbolRelation.Add(">", SynSetRelation.Cause);
            verbSymbolRelation.Add("^", SynSetRelation.AlsoSee);
            verbSymbolRelation.Add("$", SynSetRelation.VerbGroup);
            verbSymbolRelation.Add("+", SynSetRelation.DerivationallyRelated);
            verbSymbolRelation.Add(";c", SynSetRelation.TopicDomain);
            verbSymbolRelation.Add(";r", SynSetRelation.RegionDomain);
            verbSymbolRelation.Add(";u", SynSetRelation.UsageDomain);
            posSymbolRelation.Add(WordType.Verb, verbSymbolRelation);

            // adjective relations
            Dictionary<string, SynSetRelation> adjectiveSymbolRelation = new Dictionary<string, SynSetRelation>(StringComparer.OrdinalIgnoreCase);
            adjectiveSymbolRelation.Add("!", SynSetRelation.Antonym);
            adjectiveSymbolRelation.Add("&", SynSetRelation.SimilarTo);
            adjectiveSymbolRelation.Add("<", SynSetRelation.ParticipleOfVerb);
            adjectiveSymbolRelation.Add(@"\", SynSetRelation.Pertainym);
            adjectiveSymbolRelation.Add("=", SynSetRelation.Attribute);
            adjectiveSymbolRelation.Add("^", SynSetRelation.AlsoSee);
            adjectiveSymbolRelation.Add(";c", SynSetRelation.TopicDomain);
            adjectiveSymbolRelation.Add(";r", SynSetRelation.RegionDomain);
            adjectiveSymbolRelation.Add(";u", SynSetRelation.UsageDomain);
            adjectiveSymbolRelation.Add("+", SynSetRelation.DerivationallyRelated);  // not in documentation
            posSymbolRelation.Add(WordType.Adjective, adjectiveSymbolRelation);

            // adverb relations
            Dictionary<string, SynSetRelation> adverbSymbolRelation = new Dictionary<string, SynSetRelation>(StringComparer.OrdinalIgnoreCase);
            adverbSymbolRelation.Add("!", SynSetRelation.Antonym);
            adverbSymbolRelation.Add(@"\", SynSetRelation.DerivedFromAdjective);
            adverbSymbolRelation.Add(";c", SynSetRelation.TopicDomain);
            adverbSymbolRelation.Add(";r", SynSetRelation.RegionDomain);
            adverbSymbolRelation.Add(";u", SynSetRelation.UsageDomain);
            adverbSymbolRelation.Add("+", SynSetRelation.DerivationallyRelated);  // not in documentation
            posSymbolRelation.Add(WordType.Adverb, adverbSymbolRelation);
        }

        /// <summary>
        /// Gets the relation for a given POS and symbol
        /// </summary>
        /// <param name="pos">POS to get relation for</param>
        /// <param name="symbol">Symbol to get relation for</param>
        /// <returns>SynSet relation</returns>
        internal static SynSetRelation GetSynSetRelation(WordType pos, string symbol)
        {
            return posSymbolRelation[pos][symbol];
        }

        /// <summary>
        /// Gets the part-of-speech associated with a file path
        /// </summary>
        /// <param name="path">Path to get POS for</param>
        /// <returns>POS</returns>
        private static WordType GetFilePOS(string path)
        {
            WordType pos;
            string extension = Path.GetExtension(path).Trim('.');
            if (extension == "adj")
                pos = WordType.Adjective;
            else if (extension == "adv")
                pos = WordType.Adverb;
            else if (extension == "noun")
                pos = WordType.Noun;
            else if (extension == "verb")
                pos = WordType.Verb;
            else
                throw new Exception("Unrecognized data file extension:  " + extension);

            return pos;
        }

        /// <summary>
        /// Gets synset shells from a word index line. A synset shell is an instance of SynSet with only the POS and Offset
        /// members initialized. These members are enough to look up the full synset within the corresponding data file. This
        /// method is static to prevent inadvertent references to a current WordNetEngine, which should be passed via the 
        /// corresponding parameter.
        /// </summary>
        /// <param name="wordIndexLine">Word index line from which to get synset shells</param>
        /// <param name="pos">POS of the given index line</param>
        /// <param name="mostCommonSynSet">Returns the most common synset for the word</param>
        /// <returns>Synset shells for the given index line</returns>
        private static List<SynSet> GetSynSetShells(string wordIndexLine, WordType pos, out SynSet mostCommonSynSet)
        {
            List<SynSet> synsets = new List<SynSet>();
            mostCommonSynSet = null;

            // get number of synsets
            string[] parts = wordIndexLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int numSynSets = int.Parse(parts[2]);

            // grab each synset shell, from last to first
            int firstOffsetIndex = parts.Length - numSynSets;
            for (int i = parts.Length - 1; i >= firstOffsetIndex; --i)
            {
                // create synset
                int offset = int.Parse(parts[i]);

                // add synset to collection                        
                SynSet synset = new SynSet(pos, offset);
                synsets.Add(synset);

                // if this is the last synset offset to get (since we grabbed them in reverse order), record it as the most common synset
                if (i == firstOffsetIndex)
                {
                    mostCommonSynSet = synset;
                }
            }

            if (mostCommonSynSet == null)
            {
                throw new Exception("Failed to get most common synset");
            }
            return synsets;
        }

        private Dictionary<WordType, Dictionary<string, List<SynSet>>> posWordSynSets;   // in-memory pos-word synsets lookup
        private Lazy<Dictionary<WordType, List<string>>> allWords;
        private Dictionary<string, SynSet> idSynset;                               // in-memory id-synset lookup where id is POS:Offset

        /// <summary>
        /// Gets the WordNet data distribution directory
        /// </summary>
        public string WordNetDirectory { get; private set; }

        /// <summary>
        /// Gets all words in WordNet, organized by POS.
        /// </summary>
        public Dictionary<WordType, List<string>> AllWords
        {
            get
            {
                return allWords.Value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wordNetDirectory">Path to WorNet directory (the one with the data and index files in it)</param>
        public WordNetEngine(string wordNetDirectory)
        {
            WordNetDirectory = wordNetDirectory;
            Load();
            InitLazyValues();
        }

        private void InitLazyValues()
        {
            allWords = new Lazy<Dictionary<WordType, List<string>>>(
                () =>
                    {
                        Dictionary<WordType, List<string>> posWords = new Dictionary<WordType, List<string>>();
                        // grab words from in-memory index
                        foreach (WordType pos in posWordSynSets.Keys)
                        {
                            posWords.Add(pos, new List<string>(posWordSynSets[pos].Keys));
                        }
                        return posWords;
                    });
        }

        private void Load()
        {
            if (!Directory.Exists(WordNetDirectory))
            {
                throw new DirectoryNotFoundException("Non-existent WordNet directory:  " + WordNetDirectory);
            }

            // get data and index paths
            string[] dataPaths = {
                                         Path.Combine(WordNetDirectory, "data.adj"),
                                         Path.Combine(WordNetDirectory, "data.adv"),
                                         Path.Combine(WordNetDirectory, "data.noun"),
                                         Path.Combine(WordNetDirectory, "data.verb")
                                     };

            string[] indexPaths = new[]
                                      {
                                          Path.Combine(WordNetDirectory, "index.adj"),
                                          Path.Combine(WordNetDirectory, "index.adv"),
                                          Path.Combine(WordNetDirectory, "index.noun"),
                                          Path.Combine(WordNetDirectory, "index.verb")
                                      };

            // make sure all files exist
            foreach (string path in dataPaths.Union(indexPaths))
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("Failed to find WordNet file:  " + path);
                }
            }

            string sortFlagPath = Path.Combine(WordNetDirectory, ".sorted_for_dot_net");
            if (!File.Exists(sortFlagPath))
            {
                /* make sure the index files are sorted according to the current sort order. the index files in the
                 * wordnet distribution are sorted in the order needed for (presumably) the java api, which uses
                 * a different sort order than the .net runtime. thus, unless we resort the lines in the index 
                 * files, we won't be able to do a proper binary search over the data. */
                foreach (string indexPath in indexPaths)
                {
                    // create temporary file for sorted lines
                    string tempPath = Path.GetTempFileName();
                    using (StreamWriter tempFile = new StreamWriter(tempPath))
                    {

                        // get number of words (lines) in file
                        int numWords = 0;
                        using (TextReader indexFile = new StreamReader(indexPath))
                        {
                            string line;
                            while ((line = indexFile.ReadLine()) != null)
                            {
                                if (!line.StartsWith(" "))
                                {
                                    ++numWords;
                                }
                            }
                        }

                        // get lines in file, sorted by first column (i.e., the word)
                        Dictionary<string, string> wordLine = new Dictionary<string, string>(numWords, StringComparer.OrdinalIgnoreCase);
                        using (StreamReader indexFile = new StreamReader(indexPath))
                        {
                            string line;
                            while ((line = indexFile.ReadLine()) != null)
                            {
                                // write header lines to temp file immediately
                                if (line.StartsWith(" "))
                                {
                                    tempFile.WriteLine(line);
                                }
                                else
                                {
                                    // trim useless blank spaces from line and map line to first column
                                    line = line.Trim();
                                    wordLine.Add(line.Substring(0, line.IndexOf(' ')), line);
                                }
                            }
                        }

                        // get sorted words
                        List<string> sortedWords = new List<string>(wordLine.Count);
                        sortedWords.AddRange(wordLine.Keys);
                        sortedWords.Sort();

                        // write lines sorted by word
                        foreach (string word in sortedWords)
                        {
                            tempFile.WriteLine(wordLine[word]);
                        }

                        tempFile.Close();
                    }

                    // replace original index file with properly sorted one
                    File.Delete(indexPath);
                    File.Move(tempPath, indexPath);
                }

                // create flag file, indicating that we've sorted the data
                using (StreamWriter sortFlagFile = new StreamWriter(sortFlagPath))
                {
                    sortFlagFile.WriteLine(
                        "This file serves no purpose other than to indicate that the WordNet distribution data in the current directory has been sorted for use by the .NET API.");
                    sortFlagFile.Close();
                }
            }


            // pass 1:  get total number of synsets
            int totalSynsets = 0;
            foreach (string dataPath in dataPaths)
            {
                // scan synset data file for lines that don't start with a space...these are synset definition lines
                using (StreamReader dataFile = new StreamReader(dataPath))
                {
                    string line;
                    while ((line = dataFile.ReadLine()) != null)
                    {
                        int firstSpace = line.IndexOf(' ');
                        if (firstSpace > 0)
                        {
                            ++totalSynsets;
                        }
                    }
                }
            }

            // pass 2:  create synset shells (pos and offset only)
            idSynset = new Dictionary<string, SynSet>(totalSynsets, StringComparer.OrdinalIgnoreCase);
            foreach (string dataPath in dataPaths)
            {
                WordType pos = GetFilePOS(dataPath);

                // scan synset data file
                using (StreamReader dataFile = new StreamReader(dataPath))
                {
                    string line;
                    while ((line = dataFile.ReadLine()) != null)
                    {
                        int firstSpace = line.IndexOf(' ');
                        if (firstSpace > 0)
                        {
                            // get offset and create synset shell
                            int offset = int.Parse(line.Substring(0, firstSpace));
                            SynSet synset = new SynSet(pos, offset);

                            idSynset.Add(synset.ID, synset);
                        }
                    }
                }
            }

            // pass 3:  instantiate synsets (hooks up relations, set glosses, etc.)
            foreach (string dataPath in dataPaths)
            {
                WordType pos = GetFilePOS(dataPath);

                // scan synset data file
                using (StreamReader dataFile = new StreamReader(dataPath))
                {
                    string line;
                    while ((line = dataFile.ReadLine()) != null)
                    {
                        int firstSpace = line.IndexOf(' ');
                        if (firstSpace > 0)
                        {
                            // instantiate synset defined on current line, using the instantiated synsets for all references
                            idSynset[pos + ":" + int.Parse(line.Substring(0, firstSpace))].Instantiate(line, idSynset);
                        }
                    }
                }
            }

            // organize synsets by pos and words...also set most common synset for word-pos pairs that have multiple synsets
            posWordSynSets = new Dictionary<WordType, Dictionary<string, List<SynSet>>>();
            foreach (string indexPath in indexPaths)
            {
                WordType pos = GetFilePOS(indexPath);

                posWordSynSets.GetItemCreate(pos);

                // scan word index file, skipping header lines
                using (StreamReader indexFile = new StreamReader(indexPath))
                {
                    string line;
                    while ((line = indexFile.ReadLine()) != null)
                    {
                        int firstSpace = line.IndexOf(' ');
                        if (firstSpace > 0)
                        {
                            // grab word and synset shells, along with the most common synset
                            string word = line.Substring(0, firstSpace);
                            List<SynSet> synsets = GetSynSetShells(line, pos, out SynSet mostCommonSynSet);

                            // set flag on most common synset if it's ambiguous
                            if (synsets.Count > 1)
                            {
                                idSynset[mostCommonSynSet.ID].SetAsMostCommonSynsetFor(word);
                            }
                            // use reference to the synsets that we instantiated in our three-pass routine above
                            posWordSynSets[pos].Add(word, new List<SynSet>(synsets.Count));
                            foreach (SynSet synset in synsets)
                            {
                                posWordSynSets[pos][word].Add(idSynset[synset.ID]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a synset
        /// </summary>
        /// <returns>SynSet</returns>
        public SynSet GetSynSet(WordType pos, int id)
        {
            return idSynset[pos + ":" + id];
        }

        public string[] GetSynonyms(string word, WordType pos)
        {
            var mostCommon = GetMostCommonSynSet(word, pos);
            if (mostCommon == null)
            {
                return new string[] {};
            }

            Dictionary<string, string> table = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in mostCommon.Words)
            {
                string wordItem = item.Replace('_', ' ');
                table[wordItem] = wordItem;
            }
            foreach (var similar in mostCommon.GetRelatedSynSets(SynSetRelation.SimilarTo, true))
            {
                foreach (var item in similar.Words)
                {
                    string wordItem = item.Replace('_', ' ');
                    table[wordItem] = wordItem;
                }
            }
            return table.Values.ToArray();
        }

        public string[] GetAntonyms(string word, WordType pos)
        {
            var mostCommon = GetMostCommonSynSet(word, pos);
            if (mostCommon == null)
            {
                return new string[] { };
            }

            Dictionary<string, string> table = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var similar in mostCommon.GetLexicalRelationships(SynSetRelation.Antonym, true))
            {
                foreach (var item in similar.Words)
                {
                    string wordItem = item.Replace('_', ' ');
                    table[wordItem] = wordItem;
                }
            }
         
            return table.Values.ToArray();
        }

        /// <summary>
        /// Gets all synsets for a word, optionally restricting the returned synsets to one or more parts of speech. This
        /// method does not perform any morphological analysis to match up the given word. It does, however, replace all 
        /// spaces with underscores and call String.ToLower to normalize case.
        /// </summary>
        /// <param name="word">Word to get SynSets for. This method will replace all spaces with underscores and
        /// call ToLower() to normalize the word's case.</param>
        /// <param name="posRestriction">POSs to search. Cannot contain POS.None. Will search all POSs if no restriction
        /// is given.</param>
        /// <returns>Set of SynSets that contain word</returns>
        public List<SynSet> GetSynSets(string word, params WordType[] posRestriction)
        {
            // use all POSs if none are supplied
            if (posRestriction == null || posRestriction.Length == 0)
            {
                posRestriction = new[] { WordType.Adjective, WordType.Adverb, WordType.Noun, WordType.Verb };
            }
            List<WordType> posSet = new List<WordType>(posRestriction);
            if (posSet.Contains(WordType.Unknown))
            {
                throw new Exception("Invalid SynSet POS request:  " + WordType.Unknown);
            }
            // all words are lower case and space-replaced
            word = word.ToLower().Replace(' ', '_');

            // gather synsets for each POS
            List<SynSet> allSynsets = new List<SynSet>();
            foreach (WordType pos in posSet)
            {
                // read instantiated synsets from memory
                if (posWordSynSets[pos].TryGetValue(word, out List<SynSet> synsets))
                {
                    // optimization:  if there are no more parts of speech to check, we have all the synsets - so set the return collection and make it read-only. this is faster than calling AddRange on a set.
                    if (posSet.Count == 1)
                    {
                        allSynsets = synsets;
                    }
                    else
                    {
                        allSynsets.AddRange(synsets);
                    }
                }
            }
            return allSynsets;
        }

        /// <summary>
        /// Gets the most common synset for a given word/pos pair. This is only available for memory-based
        /// engines (see constructor).
        /// </summary>
        /// <param name="word">Word to get SynSets for. This method will replace all spaces with underscores and
        /// will call String.ToLower to normalize case.</param>
        /// <param name="pos">Part of speech to find</param>
        /// <returns>Most common synset for given word/pos pair</returns>
        public SynSet GetMostCommonSynSet(string word, WordType pos)
        {
            // all words are lower case and space-replaced...we need to do this here, even though it gets done in GetSynSets (we use it below)
            word = word.ToLower().Replace(' ', '_');

            // get synsets for word-pos pair
            List<SynSet> synsets = GetSynSets(word, pos);

            // get most common synset
            SynSet mostCommon = null;
            if (synsets.Count == 1)
            {
                return synsets.First();
            }
            if (synsets.Count > 1)
            {
                // one (and only one) of the synsets should be flagged as most common
                foreach (SynSet synset in synsets)
                {
                    if (synset.IsMostCommonSynsetFor(word))
                    {
                        if (mostCommon == null)
                            mostCommon = synset;
                        else
                            throw new Exception("Multiple most common synsets found");
                    }
                }
                if (mostCommon == null)
                    throw new NullReferenceException("Failed to find most common synset");
            }

            return mostCommon;
        }
    }
}