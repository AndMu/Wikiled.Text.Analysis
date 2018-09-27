namespace Wikiled.Text.Analysis.Twitter
{
    public interface IMessageCleanup
    {
        bool CleanCashTags { get; set; }

        bool CleanUrl { get; set; }

        string Cleanup(string message);
    }
}