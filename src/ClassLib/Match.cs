using System;

/// \brief Abstract match class to set a frame for sport-specific kinds of matches.
/// \details This class provides the basic structure for any kind of sports match, including common properties like MatchID, MatchDate, and Results.
public abstract class Match
{
    /// \brief Unique identifier for the match.
    public uint MatchID { get; }

    /// \brief Static counter to generate unique MatchIDs.
    private static uint MatchIDCounter = 0;

    /// \brief Date and time when the match takes place.
    public DateTime MatchDate { get; }

    /// \brief Result of team 1 in the match.
    public string? ResultTeam1 { get; }

    /// \brief Result of team 2 in the match.
    public string? ResultTeam2 { get; }

    /// \brief Constructor to initialize a match object.
    /// \param PathToMatchDataCsvFile The path to the CSV file containing match data.
    public Match(string PathToMatchDataCsvFile)
    {
        // code
    }

    /// \brief Reads the match data from a CSV file.
    /// \param PathToMatchDataCsvFile The path to the CSV file containing match data.
    /// \param MatchID The unique identifier of the match.
    /// \return An array of strings containing the match data.
    private string[] GetMatchDataFromCsvFile(string PathToMatchDataCsvFile, uint MatchID)
    {
        // code
    }
}
