using System;
using System.Collections.Generic;

/// \brief Generic class Schedule which represents a tournament.
/// \details It contains a list of all the matches which take place during the tournament.
/// \details Added to that it also contains a list of all the matches on the specific day of the tournament.
public class Schedule<M>
    where M : Match?
{
    public ScheduleTypes ScheduleID { get; }
    public SportsTypes SportType { get; }
    public List<M?>? Matches { get; }

    public Schedule(string PathToCsvFile, SportsTypes sport_type, ScheduleTypes schedule_type)
    {
        ScheduleID = schedule_type;
        SportType = sport_type;
        switch (sport_type)
        {
            case SportsTypes.Football:
                Matches = ConvertToGenericList(
                    CSVReader<FootballMatch, FootballPrediction>.GetScheduleFromCsvFile(
                        PathToCsvFile,
                        sport_type
                    )
                );
                break;
        }
    }

    private List<M?>? ConvertToGenericList(List<FootballMatch?> footballMatches)
    {
        List<M?> genericMatches = new List<M?>();
        foreach (var match in footballMatches)
        {
            genericMatches.Add(match as M);
        }
        return genericMatches;
    }

    public List<FootballMatch?> GetFootballScheduleFromCsvFile(
        string PathToCsvFile,
        SportsTypes sport_type
    )
    {
        return CSVReader<FootballMatch, FootballPrediction>.GetScheduleFromCsvFile(
            PathToCsvFile,
            sport_type
        );
    }

    public List<M> GetMatchesOnDay()
    {
        List<M>? MatchesOnDay = new List<M>();
        if (Matches != null)
        {
            foreach (M? match in Matches)
            {
                if (DateTime.Today == match?.MatchDate.Date)
                {
                    MatchesOnDay.Add(match);
                }
            }
            return MatchesOnDay;
        }
        else
        {
            return MatchesOnDay;
        }
    }
}
