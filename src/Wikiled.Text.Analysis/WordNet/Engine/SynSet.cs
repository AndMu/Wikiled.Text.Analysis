using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Wikiled.Common.Extensions;
using Wikiled.Text.Analysis.POS;

namespace Wikiled.Text.Analysis.WordNet.Engine
{
    /// <summary>
    ///     Represents a WordNet synset
    /// </summary>
    public class SynSet
    {
        private readonly int hashCode;

        // the following must never change...hashing depends on them
        private List<string> isMostCommonSynsetForWords; // words for which the current synset is the most common sense

        private Dictionary<SynSetRelation, Dictionary<SynSet, Dictionary<int, List<int>>>> lexicalRelations;

        private Dictionary<SynSetRelation, List<SynSet>> relationSynSets;

        /// <summary>
        ///     Constructor. Creates the shell of a SynSet without any actual information. To gain access to SynSet words, gloss,
        ///     and related SynSets, call SynSet.Instantiate.
        /// </summary>
        /// <param name="pos">POS of SynSet</param>
        /// <param name="offset">Byte location of SynSet definition within data file</param>
        public SynSet(WordType pos, int offset)
        {
            POS = pos;
            Offset = offset;
            Instantiated = false;

            // precompute the ID and hash code for efficiency
            ID = POS + ":" + Offset;
            hashCode = ID.GetHashCode();
        }

        /// <summary>
        ///     Gets the gloss of the current SynSet
        /// </summary>
        public string Gloss { get; private set; }

        /// <summary>
        ///     Gets the ID of this synset in the form POS:Offset
        /// </summary>
        public string ID { get; }

        /// <summary>
        ///     Gets the lexicographer file name for this synset (see the lexnames file in the WordNet distribution).
        /// </summary>
        public LexicographerFileName LexicographerFileName { get; private set; }

        /// <summary>
        ///     Gets the byte offset of synset definition within the data file
        /// </summary>
        public int Offset { get; }

        /// <summary>
        ///     Gets the POS of the current synset
        /// </summary>
        public WordType POS { get; }

        /// <summary>
        ///     Gets relations that exist between this synset and other synsets
        /// </summary>
        public IEnumerable<SynSetRelation> SynSetRelations => relationSynSets.Keys;

        /// <summary>
        ///     Gets the words in the current SynSet
        /// </summary>
        public List<string> Words { get; private set; }

        /// <summary>
        ///     Gets whether or not the current synset has been instantiated
        /// </summary>
        internal bool Instantiated { get; private set; }

        /// <summary>
        ///     Gets or sets the back-pointer used when searching WordNet
        /// </summary>
        internal SynSet SearchBackPointer { get; set; }

        /// <summary>
        ///     Checks whether two synsets are equal
        /// </summary>
        /// <param name="synset1">First synset</param>
        /// <param name="synset2">Second synset</param>
        /// <returns>True if synsets are equal, false otherwise</returns>
        public static bool operator ==(SynSet synset1, SynSet synset2)
        {
            // check object reference
            if (ReferenceEquals(synset1, synset2))
                return true;

            // check if either (but not both) are null
            if (((object)synset2 == null) ^ ((object)synset1 == null))
                return false;

            return synset1.Equals(synset2);
        }

        /// <summary>
        ///     Checks whether two synsets are unequal
        /// </summary>
        /// <param name="synset1">First synset</param>
        /// <param name="synset2">Second synset</param>
        /// <returns>True if synsets are unequal, false otherwise</returns>
        public static bool operator !=(SynSet synset1, SynSet synset2)
        {
            return !(synset1 == synset2);
        }

        /// <summary>
        ///     Checks whether the current synset equals another
        /// </summary>
        /// <param name="obj">Other synset</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SynSet))
                return false;

            SynSet synSet = obj as SynSet;

            return POS == synSet.POS && Offset == synSet.Offset;
        }

        /// <summary>
        ///     Gets hash code for this synset
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return hashCode;
        }

        /// <summary>
        ///     Gets description of synset
        /// </summary>
        /// <returns>Description</returns>
        public override string ToString()
        {
            StringBuilder desc = new StringBuilder();

            // if the synset is instantiated, include words and gloss
            if (Instantiated)
            {
                desc.Append("{");
                bool prependComma = false;
                foreach (string word in Words)
                {
                    desc.Append((prependComma ? ", " : string.Empty) + word);
                    prependComma = true;
                }

                desc.Append("}:  " + Gloss);
            }

            // if it's not instantiated, just include the ID
            else
            {
                desc.Append(ID);
            }

            return desc.ToString();
        }

        /// <summary>
        ///     Gets the closest synset that is reachable from the current and another synset along the given relations. For
        ///     example,
        ///     given two synsets and the Hypernym relation, this will return the lowest synset that is a hypernym of both synsets.
        ///     If
        ///     the hypernym hierarchy forms a tree, this will be the lowest common ancestor.
        /// </summary>
        /// <param name="synset">Other synset</param>
        /// <param name="relations">Relations to follow</param>
        /// <returns>Closest mutually reachable synset</returns>
        public SynSet GetClosestMutuallyReachableSynset(SynSet synset, IEnumerable<SynSetRelation> relations)
        {
            // avoid cycles
            List<SynSet> synsetsEncountered = new List<SynSet>();
            synsetsEncountered.Add(this);

            // start search queue
            Queue<SynSet> searchQueue = new Queue<SynSet>();
            searchQueue.Enqueue(this);

            // run search
            SynSet closest = null;
            while (searchQueue.Count > 0 &&
                   closest == null)
            {
                SynSet currSynSet = searchQueue.Dequeue();

                /* check for a path between the given synset and the current one. if such a path exists, the current
                 * synset is the closest mutually reachable synset. */
                if (synset.GetShortestPathTo(currSynSet, relations) != null)
                {
                    closest = currSynSet;
                }

                // otherwise, expand the search along the given relations
                else
                {
                    foreach (SynSet relatedSynset in currSynSet.GetRelatedSynSets(relations, false))
                    {
                        if (!synsetsEncountered.Contains(relatedSynset))
                        {
                            searchQueue.Enqueue(relatedSynset);
                            synsetsEncountered.Add(relatedSynset);
                        }
                    }
                }
            }

            return closest;
        }

        /// <summary>
        ///     Computes the depth of the current synset following a set of relations. Returns the minimum of all possible depths.
        ///     Root nodes
        ///     have a depth of zero.
        /// </summary>
        /// <param name="relations">Relations to follow</param>
        /// <returns>Depth of current SynSet</returns>
        public int GetDepth(IEnumerable<SynSetRelation> relations)
        {
            List<SynSet> synsets = new List<SynSet>();
            synsets.Add(this);

            return GetDepth(relations, synsets);
        }

        /// <summary>
        ///     Gets lexically related words for the current synset
        /// </summary>
        /// <returns>Mapping from relations to mappings from words in the current synset to related words in the related synsets</returns>
        public Dictionary<SynSetRelation, Dictionary<string, List<string>>> GetLexicallyRelatedWords()
        {
            Dictionary<SynSetRelation, Dictionary<string, List<string>>> relatedWords = new Dictionary<SynSetRelation, Dictionary<string, List<string>>>();
            foreach (SynSetRelation relation in lexicalRelations.Keys)
            {
                var itemRelation = relatedWords.GetItemCreate(relation);

                foreach (SynSet relatedSynSet in lexicalRelations[relation].Keys)
                {
                    foreach (int sourceWordIndex in lexicalRelations[relation][relatedSynSet].Keys)
                    {
                        string sourceWord = Words[sourceWordIndex - 1];
                        var itemSourceWord = itemRelation.GetItemCreate(sourceWord);

                        foreach (int targetWordIndex in lexicalRelations[relation][relatedSynSet][sourceWordIndex])
                        {
                            string targetWord = relatedSynSet.Words[targetWordIndex - 1];
                            itemSourceWord.Add(targetWord);
                        }
                    }
                }
            }

            return relatedWords;
        }

        public List<SynSet> GetLexicalRelationships(SynSetRelation relations, bool recursive)
        {
            List<SynSet> synsets = new List<SynSet>();
            GetLexicalRelationships(synsets, relations, recursive);
            return synsets;
        }

        /// <summary>
        ///     Gets the number of synsets related to the current one by the given relation
        /// </summary>
        /// <param name="relation">Relation to check</param>
        /// <returns>Number of synset related to the current one by the given relation</returns>
        public int GetRelatedSynSetCount(SynSetRelation relation)
        {
            if (!relationSynSets.ContainsKey(relation))
            {
                return 0;
            }

            return relationSynSets[relation].Count;
        }

        /// <summary>
        ///     Gets synsets related to the current synset
        /// </summary>
        /// <param name="relation">Synset relation to follow</param>
        /// <param name="recursive">Whether or not to follow the relation recursively for all related synsets</param>
        /// <returns>Synsets related to the given one by the given relation</returns>
        public List<SynSet> GetRelatedSynSets(SynSetRelation relation, bool recursive)
        {
            return GetRelatedSynSets(new[] { relation }, recursive);
        }

        /// <summary>
        ///     Gets synsets related to the current synset
        /// </summary>
        /// <param name="relations">Synset relations to follow</param>
        /// <param name="recursive">Whether or not to follow the relations recursively for all related synsets</param>
        /// <returns>Synsets related to the given one by the given relations</returns>
        public List<SynSet> GetRelatedSynSets(IEnumerable<SynSetRelation> relations, bool recursive)
        {
            List<SynSet> synsets = new List<SynSet>();

            GetRelatedSynSets(relations, recursive, synsets);

            return synsets;
        }

        /// <summary>
        ///     Gets the shortest path from the current synset to another, following the given synset relations.
        /// </summary>
        /// <param name="destination">Destination synset</param>
        /// <param name="relations">Relations to follow, or null for all relations.</param>
        /// <returns>Synset path, or null if none exists.</returns>
        public List<SynSet> GetShortestPathTo(SynSet destination, IEnumerable<SynSetRelation> relations)
        {
            if (relations == null)
                relations = Enum.GetValues(typeof(SynSetRelation)) as SynSetRelation[];

            // make sure the backpointer on the current synset is null - can't predict what other functions might do
            SearchBackPointer = null;

            // avoid cycles
            List<SynSet> synsetsEncountered = new List<SynSet>();
            synsetsEncountered.Add(this);

            // start search queue
            Queue<SynSet> searchQueue = new Queue<SynSet>();
            searchQueue.Enqueue(this);

            // run search
            List<SynSet> path = null;
            while (searchQueue.Count > 0 &&
                   path == null)
            {
                SynSet currSynSet = searchQueue.Dequeue();

                // see if we've finished the search
                if (currSynSet == destination)
                {
                    // gather synsets along path
                    path = new List<SynSet>();
                    while (currSynSet != null)
                    {
                        path.Add(currSynSet);
                        currSynSet = currSynSet.SearchBackPointer;
                    }

                    // reverse for the correct order
                    path.Reverse();
                }

                // expand the search one level
                else
                {
                    foreach (SynSet synset in currSynSet.GetRelatedSynSets(relations, false))
                    {
                        if (!synsetsEncountered.Contains(synset))
                        {
                            synset.SearchBackPointer = currSynSet;
                            searchQueue.Enqueue(synset);

                            synsetsEncountered.Add(synset);
                        }
                    }
                }
            }

            // null-out all search backpointers
            foreach (SynSet synset in synsetsEncountered)
            {
                synset.SearchBackPointer = null;
            }

            return path;
        }

        /// <summary>
        ///     Instantiates the current synset. If idSynset is non-null, related synsets references are set to those from
        ///     idSynset; otherwise, related synsets are created as shells.
        /// </summary>
        /// <param name="definition">Definition line of synset from data file</param>
        /// <param name="idSynset">Lookup for related synsets. If null, all related synsets will be created as shells.</param>
        internal void Instantiate(string definition, Dictionary<string, SynSet> idSynset)
        {
            // don't re-instantiate
            if (Instantiated)
            {
                throw new Exception("Synset has already been instantiated");
            }

            /* get lexicographer file name...the enumeration lines up precisely with the wordnet spec (see the lexnames file) except that
             * it starts with None, so we need to add 1 to the definition line's value to get the correct file name */
            int lexicographerFileNumber = int.Parse(GetField(definition, 1)) + 1;
            if (lexicographerFileNumber <= 0)
                throw new Exception("Invalid lexicographer file name number. Should be >= 1.");

            LexicographerFileName = (LexicographerFileName)lexicographerFileNumber;

            // get number of words in the synset and the start character of the word list
            int wordStart;
            int numWords = int.Parse(GetField(definition, 3, out wordStart), NumberStyles.HexNumber);
            wordStart = definition.IndexOf(' ', wordStart) + 1;

            // get words in synset
            Words = new List<string>(numWords);
            for (int i = 0; i < numWords; ++i)
            {
                int wordEnd = definition.IndexOf(' ', wordStart + 1) - 1;
                int wordLen = wordEnd - wordStart + 1;
                string word = definition.Substring(wordStart, wordLen);
                if (word.Contains(' '))
                    throw new Exception("Unexpected space in word:  " + word);

                Words.Add(word);

                // skip lex_id field
                wordStart = definition.IndexOf(' ', wordEnd + 2) + 1;
            }

            // get gloss
            Gloss = definition.Substring(definition.IndexOf('|') + 1).Trim();
            if (Gloss.Contains('|'))
                throw new Exception("Unexpected pipe in gloss");

            // get number and start of relations
            int relationCountField = 3 + Words.Count * 2 + 1;
            int relationFieldStart;
            int numRelations = int.Parse(GetField(definition, relationCountField, out relationFieldStart));
            relationFieldStart = definition.IndexOf(' ', relationFieldStart) + 1;

            // grab each related synset
            relationSynSets = new Dictionary<SynSetRelation, List<SynSet>>();
            lexicalRelations =
                new Dictionary<SynSetRelation, Dictionary<SynSet, Dictionary<int, List<int>>>>();

            for (int relationNum = 0; relationNum < numRelations; ++relationNum)
            {
                string relationSymbol = null;
                int relatedSynSetOffset = -1;
                WordType relatedSynSetPOS = WordType.Unknown;
                int sourceWordIndex = -1;
                int targetWordIndex = -1;

                // each relation has four columns
                for (int relationField = 0; relationField <= 3; ++relationField)
                {
                    int fieldEnd = definition.IndexOf(' ', relationFieldStart + 1) - 1;
                    int fieldLen = fieldEnd - relationFieldStart + 1;
                    string fieldValue = definition.Substring(relationFieldStart, fieldLen);

                    // relation symbol
                    if (relationField == 0)
                        relationSymbol = fieldValue;

                    // related synset offset
                    else if (relationField == 1)
                        relatedSynSetOffset = int.Parse(fieldValue);

                    // related synset POS
                    else if (relationField == 2)
                        relatedSynSetPOS = GetPOS(fieldValue);

                    // source/target word for lexical relation
                    else if (relationField == 3)
                    {
                        sourceWordIndex = int.Parse(fieldValue.Substring(0, 2), NumberStyles.HexNumber);
                        targetWordIndex = int.Parse(fieldValue.Substring(2), NumberStyles.HexNumber);
                    }
                    else
                        throw new Exception();

                    relationFieldStart = definition.IndexOf(' ', relationFieldStart + 1) + 1;
                }

                // get related synset...create shell if we don't have a lookup
                SynSet relatedSynSet;
                if (idSynset == null)
                {
                    relatedSynSet = new SynSet(relatedSynSetPOS, relatedSynSetOffset);
                }

                // look up related synset directly
                else
                {
                    relatedSynSet = idSynset[relatedSynSetPOS + ":" + relatedSynSetOffset];
                }

                // get relation
                SynSetRelation relation = WordNetEngine.GetSynSetRelation(POS, relationSymbol);

                // add semantic relation if we have neither a source nor a target word index
                if (sourceWordIndex == 0 &&
                    targetWordIndex == 0)
                {
                    var list = relationSynSets.GetItemCreate(relation);
                    list.Add(relatedSynSet);
                }

                // add lexical relation
                else
                {
                    var itemRelation = lexicalRelations.GetItemCreate(relation);
                    var itemRelatedSynSet = itemRelation.GetItemCreate(relatedSynSet);
                    var itemSourceWordIndex = itemRelatedSynSet.GetItemCreate(sourceWordIndex);

                    if (!itemSourceWordIndex.Contains(targetWordIndex))
                    {
                        itemSourceWordIndex.Add(targetWordIndex);
                    }
                }
            }

            Instantiated = true;
        }

        /// <summary>
        ///     Checks whether this is the most common synset for a word
        /// </summary>
        /// <param name="word">Word to check</param>
        /// <returns>True if this is the most common synset, false otherwise</returns>
        internal bool IsMostCommonSynsetFor(string word)
        {
            return isMostCommonSynsetForWords != null && isMostCommonSynsetForWords.Contains(word);
        }

        /// <summary>
        ///     Set the current synset as the most common for a word
        /// </summary>
        /// <param name="word">Word to set</param>
        internal void SetAsMostCommonSynsetFor(string word)
        {
            if (isMostCommonSynsetForWords == null)
            {
                isMostCommonSynsetForWords = new List<string>();
            }

            isMostCommonSynsetForWords.Add(word);
        }

        /// <summary>
        ///     Computes the depth of the current synset following a set of relations. Returns the minimum of all possible depths.
        ///     Root
        ///     nodes have a depth of zero.
        /// </summary>
        /// <param name="relations">Relations to follow</param>
        /// <param name="synsetsEncountered">Synsets that have already been encountered. Prevents cycles from being entered.</param>
        /// <returns>Depth of current SynSet</returns>
        private int GetDepth(IEnumerable<SynSetRelation> relations, List<SynSet> synsetsEncountered)
        {
            // get minimum depth through all relatives
            int minimumDepth = -1;
            foreach (SynSet relatedSynset in GetRelatedSynSets(relations, false))
            {
                if (!synsetsEncountered.Contains(relatedSynset))
                {
                    // add this before recursing in order to avoid cycles
                    synsetsEncountered.Add(relatedSynset);

                    // get depth from related synset
                    int relatedDepth = relatedSynset.GetDepth(relations, synsetsEncountered);

                    // use depth if it's the first or it's less than the current best
                    if (minimumDepth == -1 ||
                        relatedDepth < minimumDepth)
                        minimumDepth = relatedDepth;
                }
            }

            // depth is one plus minimum depth through any relative synset...for synsets with no related synsets, this will be zero
            return minimumDepth + 1;
        }

        /// <summary>
        ///     Gets a space-delimited field from a synset definition line
        /// </summary>
        /// <param name="line">SynSet definition line</param>
        /// <param name="fieldNum">Number of field to get</param>
        /// <returns>Field value</returns>
        private string GetField(string line, int fieldNum)
        {
            int dummy;
            return GetField(line, fieldNum, out dummy);
        }

        /// <summary>
        ///     Gets a space-delimited field from a synset definition line
        /// </summary>
        /// <param name="line">SynSet definition line</param>
        /// <param name="fieldNum">Number of field to get</param>
        /// <param name="startIndex">Start index of field within the line</param>
        /// <returns>Field value</returns>
        private string GetField(string line, int fieldNum, out int startIndex)
        {
            if (fieldNum < 0)
            {
                throw new Exception("Invalid field number:  " + fieldNum);
            }

            // scan fields until we hit the one we want
            int currField = 0;
            startIndex = 0;
            while (true)
            {
                if (currField == fieldNum)
                {
                    // get the end of the field
                    int endIndex = line.IndexOf(' ', startIndex + 1) - 1;

                    // watch out for end of line
                    if (endIndex < 0)
                        endIndex = line.Length - 1;

                    // get length of field
                    int fieldLen = endIndex - startIndex + 1;

                    // return field value
                    return line.Substring(startIndex, fieldLen);
                }

                // move to start of next field (one beyond next space)
                startIndex = line.IndexOf(' ', startIndex) + 1;

                // if there are no more spaces and we haven't found the field, the caller requested an invalid field
                if (startIndex == 0)
                    throw new Exception("Failed to get field number:  " + fieldNum);

                ++currField;
            }
        }

        private void GetLexicalRelationships(List<SynSet> synsets, SynSetRelation relation, bool recursive)
        {
            if (!lexicalRelations.ContainsKey(relation))
            {
                return;
            }

            foreach (var synset in lexicalRelations[relation].Keys)
            {
                // only add synset if it isn't already present (wordnet contains cycles)
                if (!synsets.Contains(synset))
                {
                    synsets.Add(synset);
                }
            }
        }

        /// <summary>
        ///     Gets the POS from its code
        /// </summary>
        /// <param name="pos">POS code</param>
        /// <returns>POS</returns>
        private WordType GetPOS(string pos)
        {
            WordType relatedPOS;
            if (pos == "n")
            {
                relatedPOS = WordType.Noun;
            }
            else if (pos == "v")
            {
                relatedPOS = WordType.Verb;
            }
            else if (pos == "a" ||
                     pos == "s")
            {
                relatedPOS = WordType.Adjective;
            }
            else if (pos == "r")
            {
                relatedPOS = WordType.Adverb;
            }
            else
            {
                throw new Exception("Unexpected POS:  " + pos);
            }

            return relatedPOS;
        }

        /// <summary>
        ///     Private version of GetRelatedSynSets that avoids cyclic paths in WordNet. The current synset must itself be
        ///     instantiated.
        /// </summary>
        /// <param name="relations">Synset relations to get</param>
        /// <param name="recursive">Whether or not to follow the relation recursively for all related synsets</param>
        /// <param name="currSynSets">Current collection of synsets, which we'll add to.</param>
        private void GetRelatedSynSets(IEnumerable<SynSetRelation> relations, bool recursive, List<SynSet> currSynSets)
        {
            // try each relation
            foreach (SynSetRelation relation in relations)
            {
                if (relationSynSets.ContainsKey(relation))
                {
                    foreach (SynSet relatedSynset in relationSynSets[relation])
                    {
                        // only add synset if it isn't already present (wordnet contains cycles)
                        if (!currSynSets.Contains(relatedSynset))
                        {
                            currSynSets.Add(relatedSynset);

                            if (recursive)
                            {
                                relatedSynset.GetRelatedSynSets(relations, true, currSynSets);
                            }
                        }
                    }
                }
            }
        }
    }
}
