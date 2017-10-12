namespace Wikiled.Text.Analysis.SymSpell
{
    public class SuggestItem
    {
        public SuggestItem(string term, long count, int distance)
        {
            Term = term;
            Distance = distance;
            Count = count;
        }

        public string Term { get; }

        public int Distance { get; }

        public long Count { get; }

        public override bool Equals(object obj) => base.Equals(obj);

        protected bool Equals(SuggestItem other)
        {
            return string.Equals(Term, other.Term);
        }

        public override int GetHashCode()
        {
            return Term.GetHashCode();
        }

        public static bool operator ==(SuggestItem left, SuggestItem right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SuggestItem left, SuggestItem right)
        {
            return !Equals(left, right);
        }
    }
}