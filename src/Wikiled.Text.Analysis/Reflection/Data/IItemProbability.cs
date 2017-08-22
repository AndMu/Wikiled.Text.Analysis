namespace Wikiled.Text.Analysis.Reflection.Data
{
    public interface IItemProbability<out T>
    {
        T Data { get; }

        double Probability { get; set; }
    }
}