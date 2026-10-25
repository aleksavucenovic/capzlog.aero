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

            Console.WriteLine(flight);

            return flight;
        }

    }
}
