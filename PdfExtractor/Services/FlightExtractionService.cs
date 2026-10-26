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
        private readonly CrewBriefingParser _crewBriefingParser = new();


        public FlightGroup Extract(Stream pdfStream)
        {
            var flightGroup = new FlightGroup();

            var pages = _pdfReader.Read(pdfStream);

            foreach (var page in pages)
            {
                if (_ofpParser.IsOfpPage(page.Text))
                {
                    var flight = _ofpParser.Parse(page.Text);
                    flightGroup.Flights.Add(flight);
                }
            }


            foreach (var page in pages)
            {
                if (!_crewBriefingParser.IsCrewBriefingPage(page.Text))
                    continue;

                var flight = _crewBriefingParser.FindFlight(page.Text, flightGroup.Flights);

                Console.WriteLine("\n================ CREW BRIEFING MATCH ================");
                Console.WriteLine($"PDF Page: {page.PageNumber}");

                if (flight != null)
                {
                    Console.WriteLine("MATCH FOUND: YES");
                    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                    Console.WriteLine($"ATC Call Sign: {flight.AtcCallSign}");
                    Console.WriteLine($"Registration: {flight.AircraftRegistration}");
                    Console.WriteLine($"Date: {flight.FlightDate:yyyy-MM-dd}");
                    Console.WriteLine($"Route: {flight.DepartureAirport?.IcaoCode} -> {flight.ArrivalAirport?.IcaoCode}");
                }
                else
                {
                    Console.WriteLine("MATCH FOUND: NO");
                    Console.WriteLine("No matching OFP flight found for this Crew Briefing page.");
                }

                Console.WriteLine("\n====================================++++++++++++====================================");

                if (flight == null)
                {
                    flightGroup.Errors.Add(new ExtractionError
                    {
                        Code = "CREW_BRIEFING_FLIGHT_NOT_FOUND",
                        Message = "Crew briefing page has no matching OFP flight.",
                        PageNumber = page.PageNumber
                    });
                    continue;
                }

                _crewBriefingParser.Parse(page.Text, flight);
            }


            foreach (var flight in flightGroup.Flights)
            {
                Console.WriteLine("\n====================================");
                Console.WriteLine($"Flight: {flight.FlightNumber }");
                Console.WriteLine("====================================");

                Console.WriteLine($"Date: {flight.FlightDate:yyyy-MM-dd}");
                Console.WriteLine($"Registration: {flight.AircraftRegistration}");
                Console.WriteLine($"ATC Call Sign: {flight.AtcCallSign}");

                Console.WriteLine($"Departure: {flight.DepartureAirport?.IcaoCode} {flight.DepartureAirport?.IataCode} {flight.DepartureAirport?.Name}");
                Console.WriteLine($"Arrival: {flight.ArrivalAirport?.IcaoCode} {flight.ArrivalAirport?.IataCode} {flight.ArrivalAirport?.Name}");
                Console.WriteLine($"ALTN1: {flight.AlternateAirport1?.IcaoCode} {flight.AlternateAirport1?.IataCode} {flight.AlternateAirport1?.Name}");

                Console.WriteLine($"Departure Time: {flight.DepartureTime:HH:mm}");
                Console.WriteLine($"Arrival Time: {flight.ArrivalTime:HH:mm}");
                Console.WriteLine($"Zero Fuel Mass: {flight.ZeroFuelMassKg} kg");

                Console.WriteLine($"Time to Destination: {flight.TimeToDestination}");
                Console.WriteLine($"Fuel to Destination: {flight.FuelToDestinationTons} tons");
                Console.WriteLine($"Time to Alternate: {flight.TimeToAlternate}");
                Console.WriteLine($"Fuel to Alternate: {flight.FuelToAlternateTons} tons");
                Console.WriteLine($"Minimum Fuel Required: {flight.MinimumFuelRequiredTons} tons");

                Console.WriteLine($"First Navigation Point: {flight.FirstNavigationPoint}");
                Console.WriteLine($"Last Navigation Point: {flight.LastNavigationPoint}");
                Console.WriteLine($"Gain/Loss Per Ton: {flight.GainLossPerTon}");

                Console.WriteLine("\n--- Crew Briefing ---");
                Console.WriteLine($"Business Passengers: {flight.BusinessPassengers}");
                Console.WriteLine($"Economy Passengers: {flight.EconomyPassengers}");
                Console.WriteLine($"Dry Operating Weight: {flight.DryOperatingWeightKg} kg");
                Console.WriteLine($"Dry Operating Index: {flight.DryOperatingIndex}");

                Console.WriteLine("\n--- Crew Members ---");

                foreach (var member in flight.CrewMembers)
                {
                    Console.WriteLine(
                        $"{member.FunctionCode} | {member.ThreeLetterCode} | " +
                        $"{member.FullName} | {member.Role}"
                    );
                }
            }


            return flightGroup;
        }
    }
}
