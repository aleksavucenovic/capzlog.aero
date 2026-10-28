using PdfExtractor.Utils;

namespace PdfExtractor.Tests;

public class HelperTests
{
    [Fact]
    public void ExtractValue_ReturnsValue_WhenPatternMatches()
    {
        var text = "FltNr: LX1612 Reg.: HBJVY";

        var result = Helper.ExtractValue(text, @"FltNr:\s*(\S+)");

        Assert.Equal("LX1612", result);
    }

    [Fact]
    public void ExtractValue_ReturnsNull_WhenPatternDoesNotMatch()
    {
        var result = Helper.ExtractValue("nothing useful here", @"FltNr:\s*(\S+)");

        Assert.Null(result);
    }

    [Fact]
    public void ExtractInt_ParsesNumber()
    {
        var result = Helper.ExtractInt("ZFM: 34066", @"ZFM:\s*(\d+)");

        Assert.Equal(34066, result);
    }

    [Fact]
    public void ExtractTime_ParsesTime()
    {
        var result = Helper.ExtractTime("STD: 08:00", @"STD:\s*(\d{2}:\d{2})");

        Assert.Equal(new TimeOnly(8, 0), result);
    }

    [Fact]
    public void ExtractDate_ParsesDefaultFormat()
    {
        var result = Helper.ExtractDate("Date: 19Mar24", @"Date:\s*(\d{2}[A-Za-z]{3}\d{2})");

        Assert.Equal(new DateOnly(2024, 3, 19), result);
    }

    [Fact]
    public void ExtractDate_ParsesCustomFormat()
    {
        var result = Helper.ExtractDate(
            "19.Mar.2024",
            @"(\d{2}\.[A-Za-z]{3}\.\d{4})",
            "dd.MMM.yyyy");

        Assert.Equal(new DateOnly(2024, 3, 19), result);
    }

    [Fact]
    public void ExtractInt_ReturnsNull_WhenPatternDoesNotMatch()
    {
        var result = Helper.ExtractInt("no number here", @"ZFM:\s*(\d+)");

        Assert.Null(result);
    }

    [Fact]
    public void ExtractTime_ReturnsNull_WhenTimeIsInvalid()
    {
        var result = Helper.ExtractTime("STD: 99:99", @"STD:\s*(\d{2}:\d{2})");

        Assert.Null(result);
    }

    [Fact]
    public void ExtractDate_ReturnsNull_WhenDateIsInvalid()
    {
        var result = Helper.ExtractDate("Date: 99Xyz24", @"Date:\s*(\d{2}[A-Za-z]{3}\d{2})");

        Assert.Null(result);
    }
}
