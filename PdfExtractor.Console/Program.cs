using PdfExtractor.Services;

var pdfPath = Path.Combine(
    AppContext.BaseDirectory,
    "Samples",
    "sample.pdf"
);

using var stream = File.OpenRead(pdfPath);

var extractor = new FlightExtractionService();

var result = extractor.Extract(stream);

foreach (var error in result.Errors)
{
    Console.WriteLine(
        $"[{error.Code}] Page: {error.PageNumber}, " +
        $"Flight: {error.FlightNumber}, Message: {error.Message}"
    );
}
