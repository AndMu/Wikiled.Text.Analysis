using System;
using System.Linq;
using System.Text;
using Wikiled.Text.Analysis.Emojis;

namespace Wikiled.Text.Analysis.Twitter
{
    public class MessageCleanup : IMessageCleanup
    {
        private readonly Extractor extractor = new Extractor();

        private readonly EmojyCleanup emojyCleanup = new EmojyCleanup();

        public bool CleanCashTags { get; set; } = true;

        public bool CleanUrl { get; set; } = true;

        public bool LowerCase { get; set; } = true;

        public string Cleanup(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("message", nameof(message));
            }

            string text = message;

            if (LowerCase)
            {
                text = text.ToLower();
            }
                
            if (CleanUrl)
            {
                text = Replace(text, extractor.ExtractUrlsWithIndices(message), "URL_URL");
            }

            if (CleanCashTags)
            {
                text = Replace(text, extractor.ExtractCashtagsWithIndices(text), "INDEX_INDEX");
            }

            return emojyCleanup.Extract(text).Cleaned;
        }

        private string Replace(string message, TweetEntity[] entities, string replacement)
        {
            if (entities == null ||
               entities.Length == 0)
            {
                return message;
            }

            StringBuilder builder = new StringBuilder();
            int index = 0;
            foreach (var tweetEntity in entities.OrderBy(item => item.Start))
            {
                builder.Append(message.Substring(index, tweetEntity.Start - index));
                builder.Append(replacement);
                index = tweetEntity.End;
            }

            builder.Append(message.Substring(index));
            return builder.ToString();
        }
    }
}
