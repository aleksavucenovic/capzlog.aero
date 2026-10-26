
using PdfExtractor.Models;
using PdfExtractor.Utils;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PdfExtractor.Parsers
{
    internal class CrewBriefingParser
    {
        public bool IsCrewBriefingPage(string text)
        {
            return text.Contains("Flight Assignment / Flight Crew Briefing") &&
                   text.Contains("PAX") &&
                   text.Contains("DOW:") &&
                   text.Contains("DOI:") &&
                   text.Contains("Crew");
        }

        public Flight? FindFlight(string pageText, List<Flight> flights)
        {
            var flightNumber = Helper.ExtractValue(
                pageText,
                @"(?m)^\s*(LX\d{3,5})\s*$"
            );

            var atcCallSign = Helper.ExtractValue(
                pageText,
                @"(?m)^\s*(SWR[A-Z0-9]+)\s*$"
            );

            var registration = Helper.ExtractValue(
                pageText,
                @"(?m)^\s*(HB[A-Z0-9]+)\s*$"
            );

            var date = Helper.ExtractDate(
                pageText,
                @"(\d{2}\.[A-Za-z]{3}\.\d{4})",
                "dd.MMM.yyyy"
            );

            if (flightNumber == null ||
                atcCallSign == null ||
                registration == null ||
                date == null)
            {
                return null;
            }

            foreach (var f in flights)
            {
                Console.WriteLine($"\nChecking flight: {f.FlightNumber}");
                Console.WriteLine($"FlightNumber: {f.FlightNumber} == {flightNumber} -> {f.FlightNumber == flightNumber}");
                Console.WriteLine($"ATC: {f.AtcCallSign} == {atcCallSign} -> {f.AtcCallSign == atcCallSign}");
                Console.WriteLine($"Registration: {f.AircraftRegistration} == {registration} -> {f.AircraftRegistration == registration}");
                Console.WriteLine($"Date: {f.FlightDate:yyyy-MM-dd} == {date:yyyy-MM-dd} -> {f.FlightDate == date}");
            }

            return flights.SingleOrDefault(f =>
                f.FlightNumber == flightNumber &&
                f.AtcCallSign == atcCallSign &&
                f.AircraftRegistration == registration &&
                f.FlightDate == date
            );
        }

        public void Parse(string pageText, Flight flight)
        {
            var passengers = Regex.Match(
               pageText,
               @"C/Y[\s\S]{0,180}?(\d+)\s*/\s*(\d+)"
           );

            if (passengers.Success &&
                  int.TryParse(passengers.Groups[1].Value, out var business) &&
                  int.TryParse(passengers.Groups[2].Value, out var economy))
            {
                flight.BusinessPassengers = business;
                flight.EconomyPassengers = economy;
            }

            var weightData = Regex.Match(
                  pageText,
                  @"DOW:\s*DOI:\s*EZFW:\s*(\d+)\s*kg\s+(-?\d+(?:\.\d+)?)",
                  RegexOptions.IgnoreCase
              );

            if (weightData.Success)
            {
                if (int.TryParse(weightData.Groups[1].Value, out var weight))
                {
                    flight.DryOperatingWeightKg = weight;
                }

                if (decimal.TryParse(
                    weightData.Groups[2].Value,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var index))
                {
                    flight.DryOperatingIndex = index;
                }
            }
            else
            {
                Console.WriteLine("WARNING: DOW and DOI not found.");
            }


            flight.CrewMembers = Helper.ExtractCrewMembers(pageText);
        }
    }
}
