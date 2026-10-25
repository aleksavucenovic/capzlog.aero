using PdfExtractor.Services;

var pdfPath = Path.Combine(
    AppContext.BaseDirectory,
    "Samples",
    "sample.pdf"
);

using var stream = File.OpenRead(pdfPath);

var extractor = new FlightExtractionService();

var result = extractor.Extract(stream);

Console.WriteLine($"Flights found: {result.Flights.Count}");
