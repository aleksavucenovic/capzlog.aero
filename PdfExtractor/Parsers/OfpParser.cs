using PdfExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfExtractor.Parsers
{
    internal class OfpParser
    {
        public Flight Parse(string pageText)
        {
            var flight = new Flight();

            return flight;
        }
    }
}
