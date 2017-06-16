using System.Collections.Generic;
using Wikiled.Text.Analysis.Twitter;

namespace Wikiled.Text.Analysis.Twitter
{
    /// <summary>
    ///     Compares entities bases on start index.
    /// </summary>
    internal class StartIndexComparer : Comparer<TweetEntity>
    {
        public override int Compare(TweetEntity a, TweetEntity b)
        {
            if(a.Start > b.Start)
            {
                return 1;
            }

            if(a.Start < b.Start)
            {
                return -1;
            }

            return 0;
        }
    }
}