using System;
using System.Collections.Generic;
using System.Text;

namespace PdfExtractor.Models
{
    public class FlightGroup
    {
        public List<Flight> Flights { get; set; } = new();

        public List<ExtractionError> Errors { get; set; } = new();
    }
}
