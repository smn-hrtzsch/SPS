using System;

public class FootballMatch : Match
{
    public string? HomeTeam { get; }
    public string? AwayTeam { get; }
    private MatchTypes match_type { get; }

    public FootballMatch(string PathToMatchDataCsvFile)
        : base(PathToMatchDataCsvFile)
    {
        //code
    }
}
