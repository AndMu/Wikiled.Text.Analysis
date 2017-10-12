namespace Wikiled.Text.Analysis.SymSpell
{
    public class SuggestItem
    {
        public SuggestItem(string term, long count, int distance)
        {
            Term = term.ToLower();
            Distance = distance;
            Count = count;
        }

        public string Term { get; }

        public int Distance { get; private set; }

        public long Count { get; }

        public void IncreaseDistance()
        {
            Distance++;
        }

        public override bool Equals(object obj) => base.Equals(obj);

        public SuggestItem ShallowCopy()
        {
            return (SuggestItem)MemberwiseClone();
        }

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