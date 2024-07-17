using System;

public class FootballMatch : Match
{
    public string? HomeTeam { get; }
    public string? AwayTeam { get; }
    public byte? ResultHomeTeamPenalties { get; }
    public byte? ResultAwayTeamPenalties { get; }
    private MatchTypes match_type { get; }

    public FootballMatch(string PathToMatchDataCsvFile, int line_number)
        : base(PathToMatchDataCsvFile, line_number)
    {
        this.HomeTeam = MatchArray[1];
        this.AwayTeam = MatchArray[2];
        try
        {
            this.ResultHomeTeamPenalties = byte.Parse(MatchArray[5]);
            this.ResultAwayTeamPenalties = byte.Parse(MatchArray[6]);
        }
        catch (FormatException)
        {
            this.ResultHomeTeamPenalties = null;
            this.ResultAwayTeamPenalties = null;
        }
    }

    public override string ToString()
    {
        string mi = $"{this.MatchID}";
        string md = $"{this.MatchDate}";
        string ht = $"{this.HomeTeam}";
        string at = $"{this.AwayTeam}";
        string rt1 = $"{this.ResultTeam1}";
        string rt2 = $"{this.ResultTeam2}";
        string rhtp = $"{this.ResultHomeTeamPenalties}";
        string ratp = $"{this.ResultAwayTeamPenalties}";
        return $"{mi};{md};{ht};{at};{rt1};{rt2};{rhtp};{ratp}";
    }
}
