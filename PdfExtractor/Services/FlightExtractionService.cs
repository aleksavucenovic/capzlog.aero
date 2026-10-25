using PdfExtractor.Interfaces;
using PdfExtractor.Models;
using PdfExtractor.Parsers;
using PdfExtractor.Readers;


namespace PdfExtractor.Services
{
    public class FlightExtractionService: IFlightPdfExtractor
    {
        private readonly PdfReader _pdfReader = new();
        private readonly OfpParser _ofpParser = new();


        public FlightGroup Extract(Stream pdfStream)
        {
            var flightGroup = new FlightGroup();

            var pages = _pdfReader.Read(pdfStream);
           
            foreach (var page in pages)
            {
                if (_ofpParser.IsOfpPage(page.Text))
                {
                    var flight = _ofpParser.Parse(page.Text);

                    Console.WriteLine(
                       $"{flight.FlightNumber} | {flight.FlightDate:yyyy-MM-dd} | " +
                       $"{flight.AircraftRegistration} | " +
                       $"{flight.DepartureAirport?.IcaoCode} -> {flight.ArrivalAirport?.IcaoCode}"
                    );

                    flightGroup.Flights.Add(flight);
                }
            }

            return flightGroup;
        }
    }
}
