using System;

public class Match
{
    public uint MatchID { get; }
    private static uint MatchIDCounter = 0;
    public DateTime MatchDate { get; }
    public string? ResultTeam1 { get; }
    public string? ResultTeam2 { get; }

    public Match(string PathToMatchDataCsvFile)
    {
        // code
    }

    private string[] GetMatchDataFromCsvFile(string PathToMatchDataCsvFile, uint MatchID)
    {
        // code
    }
}
