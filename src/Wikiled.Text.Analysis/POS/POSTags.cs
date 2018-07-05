using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Text.Analysis.POS.Tags;

namespace Wikiled.Text.Analysis.POS
{
    public class POSTags
    {
        private readonly Dictionary<string, BasePOSType> typesMap =
            new Dictionary<string, BasePOSType>(StringComparer.OrdinalIgnoreCase);

        private POSTags()
        {
            var properites = GetType().GetProperties()
                .Where(item => typeof (BasePOSType).IsAssignableFrom(item.PropertyType))
                .Select(item => item.GetValue(this) as BasePOSType);

            foreach (var basePosType in properites)
            {
                Register(basePosType);
            }
        }

        public CoordinatingConjunction CC => CoordinatingConjunction.Instance;

        public Particle RP => Particle.Instance;

        public CardinalNumber CD => CardinalNumber.Instance;

        public Symbol SYM => Symbol.Instance;

        public Determiner DT => Determiner.Instance;

        public To TO => To.Instance;

        public ExistentialThere EX => ExistentialThere.Instance;

        public Interjection UH => Interjection.Instance;

        public ForeignWord FW => ForeignWord.Instance;

        public VerbBaseForm VB => VerbBaseForm.Instance;

        public PrepositionSubordinateConjunction IN => PrepositionSubordinateConjunction.Instance;

        public VerbPastTense VBD => VerbPastTense.Instance;

        public Adjective JJ => Adjective.Instance;

        public VerbGerundPresentParticiple VBG => VerbGerundPresentParticiple.Instance;

        public AdjectiveComparative JJR => AdjectiveComparative.Instance;

        public VerbPastParticiple VBN => VerbPastParticiple.Instance;

        public AdjectiveSuperlative JJS => AdjectiveSuperlative.Instance;

        public VerbNon3rdPsSingPresent VBP => VerbNon3rdPsSingPresent.Instance;

        public ListItemMarker LS => ListItemMarker.Instance;

        public Verb3rdPsSingPresent VBZ => Verb3rdPsSingPresent.Instance;

        public Modal MD => Modal.Instance;

        public WhDeterminer WDT => WhDeterminer.Instance;

        public Noun NN => Noun.Instance;

        public WhPronoun WP => WhPronoun.Instance;

        public NounProper NNP => NounProper.Instance;

        public NounProperPlural NNPS => NounProperPlural.Instance;

        public NounPlural NNS => NounPlural.Instance;

        public PossessiveWhPronoun WPS => PossessiveWhPronoun.Instance;

        public WhAdverb WRB => WhAdverb.Instance;

        public LeftOpenDoubleQuote LeftOpenDoubleQuote => LeftOpenDoubleQuote.Instance;

        public Predeterminer PDT => Predeterminer.Instance;

        public PossessiveEnding POS => PossessiveEnding.Instance;

        public Comma Comma => Comma.Instance;

        public RightCloseDoubleQuote RightCloseDoubleQuote => RightCloseDoubleQuote.Instance;

        public PronounPersonal PRP => PronounPersonal.Instance;

        public SentenceFinalPunctuation SentenceFinalPunctuation => SentenceFinalPunctuation.Instance;

        public PossessivePronoun PRPS => PossessivePronoun.Instance;

        public Colon Colon => Colon.Instance;

        public Adverb RB => Adverb.Instance;

        public DollarSign DollarSign => DollarSign.Instance;

        public AdverbComparative RBR => AdverbComparative.Instance;

        public AdverbSuperlative RBS => AdverbSuperlative.Instance;

        public PoundSign PoundSign => PoundSign.Instance;

        public LeftParenthesis LRB => LeftParenthesis.Instance;

        public AdjectivePhrase ADJP => AdjectivePhrase.Instance;

        public PrepositionalPhrase PP => PrepositionalPhrase.Instance;

        public AdverbPhrase ADVP => AdverbPhrase.Instance;

        public ConjunctionPhrase CONJP => ConjunctionPhrase.Instance;

        public VerbPhrase VP => VerbPhrase.Instance;

        public NounPhrase NP => NounPhrase.Instance;

        public UnlikeCoordinatedPhrase UCP => UnlikeCoordinatedPhrase.Instance;

        public ParticlePhrase PRT => ParticlePhrase.Instance;

        public ClauseIntroducedByASubordinatingConjunction SBAR =>
            ClauseIntroducedByASubordinatingConjunction.Instance;

        public InterjectionPhrase INTJ => InterjectionPhrase.Instance;

        public ListMarker LST => ListMarker.Instance;

        public RightParenthesis RRB => RightParenthesis.Instance;

        public WHNounPhrase WHNP => WHNounPhrase.Instance;

        public PhraseUnknown UnknownPhrase => PhraseUnknown.Instance;

        public FrangmentPhrase FRAG => FrangmentPhrase.Instance;

        public QuestionPhrase QP => QuestionPhrase.Instance;

        public NounXPhrase NX => NounXPhrase.Instance;

        public XSPhrase XS => XSPhrase.Instance;

        public Parenthetical PRN => Parenthetical.Instance;

        public WhAdverbial WHADVP => WhAdverbial.Instance;

        public WhAdjectival WHADJP => WhAdjectival.Instance;

        public SQClause SQ => SQClause.Instance;

        public SBARQ SBARQ => SBARQ.Instance;

        public DelarativeSentence S => DelarativeSentence.Instance;

        public SINV SINV => SINV.Instance;

        public WordUnknown UnknownWord => WordUnknown.Instance;

        public static POSTags Instance { get; } = new POSTags();

        private void Register(BasePOSType posType)
        {
            typesMap.Add(posType.Tag, posType);
        }

        public bool Contains(string tag)
        {
            return typesMap.ContainsKey(tag);
        }

        public BasePOSType FindType(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return WordUnknown.Instance;
            }

            if (!typesMap.ContainsKey(tag))
            {
                throw new ArgumentNullException(nameof(tag), tag);
            }

            return typesMap[tag];
        }
    }
}
