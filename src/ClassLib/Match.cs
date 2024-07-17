using System;
using System.Globalization;

/// \brief Abstract match class to set a frame for sport-specific kinds of matches.
/// \details This class provides the basic structure for any kind of sports match, including common properties like MatchID, MatchDate, and Results.
public abstract class Match
{
    /// \brief Unique identifier for the match.
    public uint MatchID { get; }

    /// \brief Date and time when the match takes place.
    public DateTime MatchDate { get; }

    /// \brief Result of team 1 in the match.
    public byte? ResultTeam1 { get; }

    /// \brief Result of team 2 in the match.
    public byte? ResultTeam2 { get; }

    public string[] MatchArray { get; }

    /// \brief Constructor to initialize a match object.
    /// \param PathToMatchDataCsvFile The path to the CSV file containing match data.
    public Match(string PathToMatchDataCsvFile, int line_number)
    {
        MatchArray = CSVReader<Match>.GetMatchDataFromCsvFile(PathToMatchDataCsvFile, line_number);

        MatchDate = DateTime.ParseExact(
            MatchArray[0],
            "dd/MM/yyyy HH:mm",
            CultureInfo.InvariantCulture
        );
        ResultTeam1 = byte.Parse(MatchArray[3]);
        ResultTeam2 = byte.Parse(MatchArray[4]);
        MatchID = (uint)GetHashCode();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(MatchDate, ResultTeam1, ResultTeam2, string.Join(",", MatchArray));
    }
}
