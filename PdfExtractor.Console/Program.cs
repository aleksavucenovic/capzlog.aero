using PdfExtractor.Models;
using PdfExtractor.Services;

var pdfPath = Path.Combine(
    AppContext.BaseDirectory,
    "Samples",
    "sample.pdf"
);

using var stream = File.OpenRead(pdfPath);

var extractor = new FlightExtractionService();

var result = extractor.Extract(stream);

foreach (var flight in result.Flights)
{
    PrintFlight(flight);
}

PrintErrors(result.Errors);

static void PrintFlight(Flight flight)
{
    Console.WriteLine("\n====================================");
    Console.WriteLine($"Flight: {flight.FlightNumber}");
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

static void PrintErrors(List<ExtractionError> errors)
{
    if (errors.Count == 0)
        return;

    Console.WriteLine("\n--- Errors ---");

    foreach (var error in errors)
    {
        Console.WriteLine(
            $"[{error.Code}] Page: {error.PageNumber}, " +
            $"Flight: {error.FlightNumber}, Message: {error.Message}"
        );
    }
}
