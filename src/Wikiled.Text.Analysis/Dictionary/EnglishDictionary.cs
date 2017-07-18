using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wikiled.Core.Utility.Arguments;
using Wikiled.Text.Analysis.Resources;
using Wikiled.Text.Analysis.WordNet.Engine;

namespace Wikiled.Text.Analysis.Dictionary
{
    public class EnglishDictionary : IWordsDictionary
    {
        private Dictionary<string, double> words;

        private readonly string datasetPath;

        public EnglishDictionary(string path, IWordNetEngine wordNetEngine)
        {
            Guard.NotNullOrEmpty(() => path, path);
            Guard.NotNull(() => wordNetEngine, wordNetEngine);
            datasetPath = path;
            Init(wordNetEngine);
        }

        private void Init(IWordNetEngine wordNetEngine)
        {
            words = ReadTextData("RawEnglish.txt", true);
            foreach (var word in wordNetEngine.AllWords)
            {
                foreach (var wordItem in word.Value)
                {
                    if (!words.ContainsKey(wordItem))
                    {
                        words.Add(wordItem, 0);
                    }
                }
            }
        }

        private Dictionary<string, double> ReadTextData(string file, bool useDefault)
        {
            return ReadTabResourceDataFile.ReadTextData(Path.Combine(datasetPath, file), useDefault);
        }

        public bool IsKnown(string word)
        {
            if (string.IsNullOrWhiteSpace(word) ||
                word.Length < 2)
            {
                return false;
            }

            return words.ContainsKey(word);
        }

        public string[] GetWords()
        {
            return words.Select(item => item.Key).Distinct().ToArray();
        }
    }
}
