using PdfExtractor.Interfaces;
using PdfExtractor.Models;
using PdfExtractor.Readers;


namespace PdfExtractor.Services
{
    public class FlightExtractionService: IFlightPdfExtractor
    {
        private readonly PdfReader _pdfReader = new();

        public FlightGroup Extract(Stream pdfStream)
        {
            var flightGroup = new FlightGroup();

            var pages = _pdfReader.Read(pdfStream);
            Console.WriteLine($"Pages read: {pages.Count}");

            return flightGroup;
        }
    }
}
