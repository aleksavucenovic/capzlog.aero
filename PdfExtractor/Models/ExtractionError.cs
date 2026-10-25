using System;
using System.Collections.Generic;
using System.Text;

namespace PdfExtractor.Models
{
    public class ExtractionError
    {
        public string Code { get; set; } = "";
        public string Message { get; set; } = "";
        public int? PageNumber { get; set; }
        public string? FlightNumber { get; set; }
        public string? FieldName { get; set; }
        public string? RawValue { get; set; }
        public string? Details { get; set; }
    }
}
