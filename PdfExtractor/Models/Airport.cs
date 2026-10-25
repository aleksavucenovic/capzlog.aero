using System;
using System.Collections.Generic;
using System.Text;

namespace PdfExtractor.Models
{
    public class Airport
    {
        public string? IcaoCode { get; set; } // ICAO airport code (e.g. LSZH)
        public string? IataCode { get; set; } // IATA airport code (e.g. ZRH)
        public string? Name { get; set; } // Airport name (e.g. ZURICH)
    }
}
