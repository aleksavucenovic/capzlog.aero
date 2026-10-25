using PdfExtractor.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfExtractor.Utils
{
    internal class Helper
    {

        public static string? ExtractValue(string text, string pattern)
        {
            var match = Regex.Match(text, pattern);

            return match.Success ? match.Groups[1].Value : null;
        }

        public static DateOnly? ExtractDate(string text, string pattern)
        {
            var dateText = ExtractValue(text, pattern);

            if (DateOnly.TryParseExact(
                dateText,
                "ddMMMyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
            {
                return date;
            }

            return null;
        }

        public static Airport? ExtractAirport(string text, string label)
        {
            var pattern = $@"(?m)^\s*{Regex.Escape(label)}:\s*([A-Z]{{4}})\s+([A-Z]{{3}})\s+(.+?)\s*$";
            var match = Regex.Match(text, pattern);

            if (!match.Success) return null;

            // TODO: Discuss with the team whether ICAO and IATA code validation is needed.
            // If required, validate both codes against a trusted airport database and verify they refer to the same airport.
            // Report unknown or mismatched codes without automatically rejecting the flight.

            return new Airport
            {
                IcaoCode = match.Groups[1].Value,
                IataCode = match.Groups[2].Value,
                Name = match.Groups[3].Value.Trim()
            };
        }

    }
}
