using System;
using System.Collections.Generic;
using System.Text;

namespace PdfExtractor.Models
{
    public class CrewMember
    {
        public string? FunctionCode { get; set; }
        public string? ThreeLetterCode { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
}
