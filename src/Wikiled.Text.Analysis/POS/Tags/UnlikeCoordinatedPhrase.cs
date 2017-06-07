namespace Wikiled.Text.Analysis.POS.Tags
{
    public class UnlikeCoordinatedPhrase : BasePOSType
    {
        private readonly static UnlikeCoordinatedPhrase instance = new UnlikeCoordinatedPhrase();

        private UnlikeCoordinatedPhrase()
        {
        }

        public override string Description
        {
            get { return "Unlike Coordinated Phrase"; }
        }

        public override WordType WordType
        {
            get { return WordType.Unknown; }
        }

        public override bool IsGroup
        {
            get
            {
                return true;
            }
        }

        public static UnlikeCoordinatedPhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "UCP"; }
        }
    }
}

