namespace Wikiled.Text.Analysis.Tokenizer.Pipelined
{
    public class StopWordItemPipeline : WordItemFilterOutPipeline
    {
        public StopWordItemPipeline() 
            : base(item => item.IsStop)
        {
        }
    }
}
