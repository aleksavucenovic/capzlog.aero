# capzlog.aero

Extract Data from a PDF File using C# and .net core

## GitHub link

Repo: `https://github.com/aleksavucenovic/capzlog.aero.git`

## What it extracts

- Flight details, airports, times, fuel, loadmass
- Crew briefing/members
- Extraction errors

## PDF processing

The project uses PdfPig to read PDF pages and extract text.
Link: `https://uglytoad.github.io/PdfPig/`

## Instructions

Requires the .NET 10 SDK.
The place for PDF samples: `PdfExtractor.Console/Samples/sample.pdf`.

Run the app from the repository root:

```bash
dotnet run --project PdfExtractor.Console
```

Run the tests:

```bash
dotnet test
```