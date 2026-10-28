using PdfExtractor.Utils;

namespace PdfExtractor.Tests;

// Tests for Helper.ExtractAirport
public class HelperAirportTests
{
    [Fact]
    public void ExtractAirport_ParsesCodesAndName()
    {
        // Text as it appears in the PDF
        var text = "From: LSZH ZRH ZURICH";

        var airport = Helper.ExtractAirport(text, "From");

        Assert.NotNull(airport);
        Assert.Equal("LSZH", airport.IcaoCode);
        Assert.Equal("ZRH", airport.IataCode);
        Assert.Equal("ZURICH", airport.Name);
    }

    [Fact]
    public void ExtractAirport_StopsNameBeforeExtraColumns()
    {
        // The PDF puts other fields on the same line after a big gap
        var text = "From: LSZH ZRH ZURICH";

        var airport = Helper.ExtractAirport(text, "From");

        Assert.NotNull(airport);
        Assert.Equal("ZURICH", airport.Name);
    }

    [Fact]
    public void ExtractAirport_ParsesNameWithSlash()
    {
        var airport = Helper.ExtractAirport("To: LIMC MXP MILAN/MALPENSA", "To");

        Assert.NotNull(airport);
        Assert.Equal("MILAN/MALPENSA", airport.Name);
    }

    [Fact]
    public void ExtractAirport_FindsTheRightLabel()
    {
        var text = "From: LSZH ZRH ZURICH\nTo: LIMC MXP MILAN/MALPENSA";

        var airport = Helper.ExtractAirport(text, "To");

        Assert.NotNull(airport);
        Assert.Equal("LIMC", airport.IcaoCode);
    }

    [Fact]
    public void ExtractAirport_ReturnsNull_WhenLabelNotFound()
    {
        var airport = Helper.ExtractAirport("From: LSZH ZRH ZURICH", "ALTN1");

        Assert.Null(airport);
    }

    [Fact]
    public void ExtractAirport_ReturnsNull_WhenCodesAreInvalid()
    {
        // ICAO code must be 4 letters
        var airport = Helper.ExtractAirport("From: LS ZRH ZURICH", "From");

        Assert.Null(airport);
    }
}
