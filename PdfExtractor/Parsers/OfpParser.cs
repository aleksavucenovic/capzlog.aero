using PdfExtractor.Models;
using PdfExtractor.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfExtractor.Parsers
{
    internal class OfpParser
    {

        public bool IsOfpPage(string text)
        {
            return text.Contains("Operational Flight Plan") &&
               text.Contains("Flight Info") &&
               text.Contains("Date:") &&
               text.Contains("From:");
        }

        public Flight Parse(string pageText)
        {
            var flight = new Flight();

            flight.FlightNumber = Helper.ExtractValue(pageText, @"FltNr:\s*(\S+)");
            flight.AircraftRegistration = Helper.ExtractValue(pageText, @"Reg\.:\s*(\S+)");
            flight.AtcCallSign = Helper.ExtractValue(pageText, @"ATC:\s*(\S+)");

            flight.FlightDate = Helper.ExtractDate(
                pageText,
                @"Date:\s*(\d{2}[A-Za-z]{3}\d{2})"
            );

            flight.DepartureAirport = Helper.ExtractAirport(pageText, "From");
            flight.ArrivalAirport = Helper.ExtractAirport(pageText, "To");
            flight.AlternateAirport1 = Helper.ExtractAirport(pageText, "ALTN1");

            flight.DepartureTime = Helper.ExtractTime(
                pageText, @"\bSTD:\s*(\d{2}:\d{2})");

            flight.ArrivalTime = Helper.ExtractTime(
                pageText, @"\bSTA:\s*(\d{2}:\d{2})");

            flight.ZeroFuelMassKg = Helper.ExtractInt(
                pageText, @"\bZFM:\s*(\d+)");

            var destination = flight.ArrivalAirport?.IcaoCode;

            if (destination != null)
            {
                var fuel = Helper.ExtractFuelRow(pageText, destination);
                flight.TimeToDestination = fuel.Time;
                flight.FuelToDestinationTons = fuel.Fuel;
            }

            var alternate = flight.AlternateAirport1?.IcaoCode;

            if (alternate != null)
            {
                var fuel = Helper.ExtractFuelRow(pageText, alternate);
                flight.TimeToAlternate = fuel.Time;
                flight.FuelToAlternateTons = fuel.Fuel;
            }

            flight.MinimumFuelRequiredTons = Helper.ExtractDecimal(
                pageText, @"(?m)^\s*MIN:\s*\d+:\d{2}\s+(\d+(?:\.\d+)?)");

            var route = Helper.ExtractValue(
                pageText, @"(?m)^\s*To DEST:\s*([^\r\n]+)");

            if (!string.IsNullOrWhiteSpace(route))
            {
                var points = route.Split(
                    ' ',
                    StringSplitOptions.RemoveEmptyEntries
                );

                flight.FirstNavigationPoint = points.FirstOrDefault();
                flight.LastNavigationPoint = points.LastOrDefault();
            }

            var gainLoss = Regex.Match(
                pageText,
                @"Gain\s*/\s*Loss:\s*(GAIN|LOSS)\s+(\d+(?:\.\d+)?)",
                RegexOptions.IgnoreCase
            );

            if (gainLoss.Success &&
                decimal.TryParse(
                    gainLoss.Groups[2].Value,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var amount))
            {
                flight.GainLossPerTon =
                    gainLoss.Groups[1].Value.Equals(
                        "LOSS",
                        StringComparison.OrdinalIgnoreCase
                    ) ? -amount : amount;
            }



            return flight;
        }

    }
}
