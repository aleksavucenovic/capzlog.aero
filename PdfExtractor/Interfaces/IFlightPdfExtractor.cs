using PdfExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PdfExtractor.Interfaces
{
    public interface IFlightPdfExtractor
    {
        FlightGroup Extract(Stream pdfStream);
    }
}
