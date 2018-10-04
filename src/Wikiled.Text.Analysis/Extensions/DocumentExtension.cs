using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Extensions
{
    public static class DocumentExtension
    {
        public static string GetTextId(this Document document)
        {
            return $"Text:{document.Text.GenerateKey()}";
        }

        public static string GetId(this Document document)
        {
            return $"Document:{document.Id}:{document.Text.GenerateKey()}";
        }
    }
}
