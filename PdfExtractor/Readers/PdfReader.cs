
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace PdfExtractor.Readers
{
    internal class PdfReader
    {
        public List<PdfPageContent> Read(Stream pdfStream)
        {
            var pages = new List<PdfPageContent>();
            using var document = PdfDocument.Open(pdfStream);

            foreach (var page in document.GetPages())
            {
                var text = ContentOrderTextExtractor.GetText(page);
                pages.Add(new PdfPageContent(page.Number, text));
            }

            return pages;
        }
    }
}
