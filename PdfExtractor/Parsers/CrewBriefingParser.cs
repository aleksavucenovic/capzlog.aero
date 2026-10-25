using PdfExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;
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

        public void Parse(string pageText, Flight flight)
        {
            
        }
    }
}
