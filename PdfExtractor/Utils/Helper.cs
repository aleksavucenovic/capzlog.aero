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

        public static DateOnly? ExtractDate(
         string text,
         string pattern,
         string format = "ddMMMyy")
        {
            var dateText = ExtractValue(text, pattern);

            if (DateOnly.TryParseExact(
                dateText,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
            {
                return date;
            }

            return null;
        }

        public static TimeOnly? ExtractTime(string text, string pattern)
        {
            var value = ExtractValue(text, pattern);

            return TimeOnly.TryParseExact(
                value,
                "HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var time
            ) ? time : null;
        }

        public static int? ExtractInt(string text, string pattern)
        {
            var value = ExtractValue(text, pattern);

            return int.TryParse(value, out var number)
                ? number
                : null;
        }

        public static decimal? ExtractDecimal(string text, string pattern)
        {
            var value = ExtractValue(text, pattern);

            return decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var number
            ) ? number : null;
        }

        public static (TimeSpan? Time, decimal? Fuel) ExtractFuelRow(string text, string airportCode)
        {
            var pattern =
                $@"(?m)^[ \t]*{Regex.Escape(airportCode)}:" +
                @"[ \t]*(\d+:\d{2})[ \t]+(\d+(?:\.\d+)?)";

            var match = Regex.Match(text, pattern);

            if (!match.Success)
                return (null, null);

            var hasTime = TimeSpan.TryParseExact(
                match.Groups[1].Value,
                @"h\:mm",
                CultureInfo.InvariantCulture,
                out var time
            );

            var hasFuel = decimal.TryParse(
                match.Groups[2].Value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out var fuel
            );

            return (
                hasTime ? time : null,
                hasFuel ? fuel : null
            );
        }

        public static Airport? ExtractAirport(string text, string label)
        {
            var pattern =
                $@"(?m)^[ \t]*{Regex.Escape(label)}:[ \t]*" +
                @"([A-Z]{4})[ \t]+([A-Z]{3})[ \t]+(.+?)(?=[ \t]{2,}|$)";
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






        public static List<CrewMember> ExtractCrewMembers(string pageText)
        {
            var members = new List<CrewMember>();

            // Find the crew table header
            var startMatch = Regex.Match(
                pageText,
                @"(?m)^[ \t]*Func[ \t]+3LC[ \t]+Name[ \t]+LX\d+[ \t]*\r?$"
            );

            if (!startMatch.Success)
            {
                Console.WriteLine("ERROR: Crew table header not found.");
                return members;
            }

            var startIndex = startMatch.Index + startMatch.Length;

            // Find the end of the crew table
            var endMatch = Regex.Match(
                pageText.Substring(startIndex),
                @"(?m)^[ \t]*Function[ \t]+Name[ \t]*\r?$"
            );

            var endIndex = endMatch.Success
                ? startIndex + endMatch.Index
                : pageText.Length;

            var crewText = pageText.Substring(
                startIndex,
                endIndex - startIndex
            );

            Console.WriteLine("\n--- CREW SECTION ---");
            Console.WriteLine(crewText);

            CrewMember? currentMember = null;

            foreach (var rawLine in crewText.Split('\n'))
            {
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Skip route line, e.g. LSZH-LIMC
                if (Regex.IsMatch(line, @"^[A-Z]{4}-[A-Z]{4}$"))
                    continue;

                // Detect a new crew member
                var match = Regex.Match(
                    line,
                    @"^(?<function>CMD|COP|CAB|SEN)\s+" +
                    @"(?<code>[A-Z]{3})\s+(?<rest>.+)$"
                );

                if (match.Success)
                {
                    var rest = match.Groups["rest"].Value.Trim();

                    var roleMatch = Regex.Match(
                        rest,
                        @"\s+(Commander|Copilot|Senior Cabin|Cabin)$"
                    );

                    var name = roleMatch.Success
                        ? rest[..roleMatch.Index].Trim()
                        : rest;

                    var role = roleMatch.Success
                        ? roleMatch.Groups[1].Value
                        : null;

                    currentMember = new CrewMember
                    {
                        FunctionCode = match.Groups["function"].Value,
                        ThreeLetterCode = match.Groups["code"].Value,
                        FullName = name,
                        Role = role
                    };

                    members.Add(currentMember);
                    continue;
                }

                if (currentMember == null)
                    continue;

                // Handle role continuation
                if (line is "Cabin" or "Senior Cabin" or "Commander" or "Copilot")
                {
                    currentMember.Role = line;
                    continue;
                }

                if (line == "Attendant")
                {
                    currentMember.Role = string.IsNullOrEmpty(currentMember.Role)
                        ? "Attendant"
                        : currentMember.Role + " Attendant";

                    continue;
                }

                // Handle name continuation, e.g. Vissotto or Schumacher-Horn
                currentMember.FullName = string.IsNullOrEmpty(currentMember.FullName)
                    ? line
                    : currentMember.FullName + " " + line;
            }

            Console.WriteLine("\n========== EXTRACTED CREW ==========");
            Console.WriteLine($"Total crew members: {members.Count}");

            foreach (var member in members)
            {
                Console.WriteLine(
                    $"{member.FunctionCode} | {member.ThreeLetterCode} | " +
                    $"{member.FullName} | {member.Role ?? "N/A"}"
                );
            }

            return members;
        }



    }
}
