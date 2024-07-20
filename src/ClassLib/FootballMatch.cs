using System;
using System.Globalization;

public class FootballMatch : Match
{
    public string? HomeTeam { get; }
    public string? AwayTeam { get; }
    public byte? ResultHomeTeamPenalties { get; }
    public byte? ResultAwayTeamPenalties { get; }
    private MatchTypes match_type { get; }

    /// \brief Parameter free constructor to use generic type M in <see cref="CSVReader"/>
    public FootballMatch() { }

    public FootballMatch(string PathToMatchDataCsvFile, int line_number, SportsTypes sport_type)
    {
        InitializeMatch(PathToMatchDataCsvFile, line_number, sport_type);
        HomeTeam = MatchArray[1];
        AwayTeam = MatchArray[2];
        try
        {
            ResultHomeTeamPenalties = byte.Parse(MatchArray[5]);
            ResultAwayTeamPenalties = byte.Parse(MatchArray[6]);
        }
        catch (FormatException)
        {
            ResultHomeTeamPenalties = null;
            ResultAwayTeamPenalties = null;
        }
    }

    private void InitializeMatch(
        string PathToMatchDataCsvFile,
        int line_number,
        SportsTypes sport_type
    )
    {
        MatchArray = CSVReader<FootballMatch>.GetMatchDataFromCsvFile(
            PathToMatchDataCsvFile,
            line_number
        );

        MatchDate = DateTime.ParseExact(
            MatchArray[0],
            "dd/MM/yyyy HH:mm",
            CultureInfo.InvariantCulture
        );
        ResultTeam1 = byte.Parse(MatchArray[3]);
        ResultTeam2 = byte.Parse(MatchArray[4]);
        SportsType = sport_type;
        MatchID = (uint)GetHashCode();
    }

    public override string ToString()
    {
        string mi = $"{MatchID}";
        string md = $"{MatchDate}";
        string ht = $"{HomeTeam}";
        string at = $"{AwayTeam}";
        string rt1 = $"{ResultTeam1}";
        string rt2 = $"{ResultTeam2}";
        string rhtp = $"{ResultHomeTeamPenalties}";
        string ratp = $"{ResultAwayTeamPenalties}";
        return $"{mi};{md};{ht};{at};{rt1};{rt2};{rhtp};{ratp}";
    }
}
