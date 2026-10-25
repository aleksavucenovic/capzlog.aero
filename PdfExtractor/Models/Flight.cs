using System;
using System.Collections.Generic;
using System.Text;

namespace PdfExtractor.Models
{
    public class Flight
    {
        // Operational Flight Plan
        // Info
        public DateOnly? FlightDate { get; set; } // Date
        public string? AircraftRegistration { get; set; } // Reg.

        public Airport? DepartureAirport { get; set; } // From
        public Airport? ArrivalAirport { get; set; } // To
        public Airport? AlternateAirport1 { get; set; } // ALTN1

        public string? FlightNumber { get; set; } // FltNr
        public string? AtcCallSign { get; set; } // ATC

        // Time
        public TimeOnly? DepartureTime { get; set; } // STD
        public TimeOnly? ArrivalTime { get; set; } // STA

        // Loadmass
        public int? ZeroFuelMassKg { get; set; } // ZFM

        // Fuel
        public TimeSpan? TimeToDestination { get; set; } // LIMC
        public decimal? FuelToDestinationTons { get; set; } // LIMC
        public TimeSpan? TimeToAlternate { get; set; } // LIML
        public decimal? FuelToAlternateTons { get; set; } // LIML
        public decimal? MinimumFuelRequiredTons { get; set; } // MIN

        // ATC Route
        public string? FirstNavigationPoint { get; set; } // To Dest:
        public string? LastNavigationPoint { get; set; } // To Dest:

        // Corrections
        public decimal? GainLossPerTon { get; set; } // Gain/Loss


        // Crew Briefing
        public int? BusinessPassengers { get; set; } // PAX C
        public int? EconomyPassengers { get; set; } // PAX Y
        public int? DryOperatingWeightKg { get; set; } // DOW
        public decimal? DryOperatingIndex { get; set; } // DOI

        public List<CrewMember> CrewMembers { get; set; } = new();
    }
}
