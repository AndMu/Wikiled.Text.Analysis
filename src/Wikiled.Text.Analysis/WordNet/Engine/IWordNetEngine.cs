using System.Collections.Generic;
using Wikiled.Text.Analysis.POS;

namespace Wikiled.Text.Analysis.WordNet.Engine
{
    public interface IWordNetEngine
    {
        /// <summary>
        /// Gets the WordNet data distribution directory
        /// </summary>
        string WordNetDirectory { get; }

        /// <summary>
        /// Gets all words in WordNet, organized by POS.
        /// </summary>
        Dictionary<WordType, List<string>> AllWords { get; }

        string[] GetSynonyms(string word, WordType pos);
        
        string[] GetAntonyms(string word, WordType pos);

        SynSet GetSynSet(WordType pos, int id);

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
        List<SynSet> GetSynSets(string word, params WordType[] posRestriction);

        /// <summary>
        /// Gets the most common synset for a given word/pos pair. This is only available for memory-based
        /// engines (see constructor).
        /// </summary>
        /// <param name="word">Word to get SynSets for. This method will replace all spaces with underscores and
        /// will call String.ToLower to normalize case.</param>
        /// <param name="pos">Part of speech to find</param>
        /// <returns>Most common synset for given word/pos pair</returns>
        SynSet GetMostCommonSynSet(string word, WordType pos);
    }
}