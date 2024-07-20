using System;

public interface IMatchFactory<M>
    where M : Match
{
    M CreateMatch(string PathToMatchDataCsvFile, int line_number, SportsTypes sport_type);
}

public class FootballMatchFactory : IMatchFactory<FootballMatch>
{
    public FootballMatch CreateMatch(string PathToMatchDataCsvFile, int line_number, SportsTypes sport_type)
    {
        return new FootballMatch(PathToMatchDataCsvFile, line_number, sport_type);
    }
}