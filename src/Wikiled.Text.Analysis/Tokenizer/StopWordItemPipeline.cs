namespace Wikiled.Text.Analysis.Tokenizer
{
    public class StopWordItemPipeline : WordItemFilterOutPipeline
    {
        public StopWordItemPipeline() 
            : base(item => item.IsStop)
        {
        }
    }
}
