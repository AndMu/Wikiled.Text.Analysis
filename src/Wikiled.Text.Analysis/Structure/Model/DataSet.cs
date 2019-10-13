namespace Wikiled.Text.Analysis.Structure.Model
{
    public class DataSet
    {
        public IProcessingTextBlock[] Positive { get; set; }

        public IProcessingTextBlock[] Negative { get; set; }
    }
}
