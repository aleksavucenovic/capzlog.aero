using PdfExtractor.Utils;

namespace PdfExtractor.Tests;

public class HelperCrewTests
{
    // Joins lines into one page text, like the PDF reader returns it
    private static string Page(params string[] lines) => string.Join("\n", lines);

    [Fact]
    public void ExtractCrewMembers_ParsesCommanderAndCopilot()
    {
        var text = Page(
            "Func 3LC Name LX1073",
            "EDDF-LSZH",
            "CMD TRW Werner Trütsch Commander",
            "COP MRL Luca Andrea Marchetti Copilot",
            "Function Name");

        var crew = Helper.ExtractCrewMembers(text);

        Assert.Equal(2, crew.Count);

        Assert.Equal("CMD", crew[0].FunctionCode);
        Assert.Equal("TRW", crew[0].ThreeLetterCode);
        Assert.Equal("Werner Trütsch", crew[0].FullName);
        Assert.Equal("Commander", crew[0].Role);

        Assert.Equal("Luca Andrea Marchetti", crew[1].FullName);
        Assert.Equal("Copilot", crew[1].Role);
    }

    [Fact]
    public void ExtractCrewMembers_JoinsRoleSplitOverTwoLines()
    {
        // "Cabin Attendant" is split into two lines in the PDF
        var text = Page(
            "Func 3LC Name LX1073",
            "EDDF-LSZH",
            "CAB MHE Helen Meier Cabin",
            "Attendant",
            "Function Name");

        var crew = Helper.ExtractCrewMembers(text);

        Assert.Single(crew);
        Assert.Equal("Helen Meier", crew[0].FullName);
        Assert.Equal("Cabin Attendant", crew[0].Role);
    }

    [Fact]
    public void ExtractCrewMembers_ParsesSeniorCabinAttendant()
    {
        var text = Page(
            "Func 3LC Name LX1073",
            "EDDF-LSZH",
            "SEN VEN Nico Verhelst Senior Cabin",
            "Attendant",
            "Function Name");

        var crew = Helper.ExtractCrewMembers(text);

        Assert.Single(crew);
        Assert.Equal("Senior Cabin Attendant", crew[0].Role);
    }

    [Fact]
    public void ExtractCrewMembers_JoinsNameSplitOverTwoLines()
    {
        var text = Page(
            "Func 3LC Name LX1613",
            "LIMC-LSZH",
            "CAB QVL Luisa Quadros",
            "Vissotto",
            "Cabin",
            "Attendant",
            "Function Name");

        var crew = Helper.ExtractCrewMembers(text);

        Assert.Single(crew);
        Assert.Equal("Luisa Quadros Vissotto", crew[0].FullName);
        Assert.Equal("Cabin Attendant", crew[0].Role);
    }

    [Fact]
    public void ExtractCrewMembers_IgnoresRouteLine()
    {
        var text = Page(
            "Func 3LC Name LX1073",
            "EDDF-LSZH",
            "CMD TRW Werner Trütsch Commander",
            "Function Name");

        var crew = Helper.ExtractCrewMembers(text);

        Assert.Single(crew);
    }

    [Fact]
    public void ExtractCrewMembers_ReturnsEmptyList_WhenHeaderNotFound()
    {
        var crew = Helper.ExtractCrewMembers("some other page");

        Assert.Empty(crew);
    }
}
