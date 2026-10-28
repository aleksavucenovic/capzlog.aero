using PdfExtractor.Utils;

namespace PdfExtractor.Tests;

public class HelperFuelTests
{
    [Fact]
    public void ExtractFuelRow_ReturnsTimeAndFuel()
    {
        var text = "LIMC: 0:48 1.7";

        var (time, fuel) = Helper.ExtractFuelRow(text, "LIMC");

        Assert.Equal(TimeSpan.FromMinutes(48), time);
        Assert.Equal(1.7m, fuel);
    }

    [Fact]
    public void ExtractFuelRow_FindsTheRightAirport()
    {
        var text = "LIMC: 0:48 1.7\nLIML: 0:20 0.8";

        var (time, fuel) = Helper.ExtractFuelRow(text, "LIML");

        Assert.Equal(TimeSpan.FromMinutes(20), time);
        Assert.Equal(0.8m, fuel);
    }

    [Fact]
    public void ExtractFuelRow_ReturnsNulls_WhenAirportNotFound()
    {
        var (time, fuel) = Helper.ExtractFuelRow("LIMC: 0:48 1.7", "LIML");

        Assert.Null(time);
        Assert.Null(fuel);
    }

    [Fact]
    public void ExtractDecimal_ParsesNumberWithDot()
    {
        var result = Helper.ExtractDecimal("DOI: 68.7", @"DOI:\s*(-?\d+(?:\.\d+)?)");

        Assert.Equal(68.7m, result);
    }

    [Fact]
    public void ExtractDecimal_ParsesNegativeNumber()
    {
        var result = Helper.ExtractDecimal("Gain/Loss: -40", @"Gain/Loss:\s*(-?\d+)");

        Assert.Equal(-40m, result);
    }

    [Fact]
    public void ExtractDecimal_ReturnsNull_WhenPatternDoesNotMatch()
    {
        var result = Helper.ExtractDecimal("nothing here", @"DOI:\s*(\d+)");

        Assert.Null(result);
    }
}
