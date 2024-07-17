using System;
using System.Collections.Generic;

/// \brief Generic class Schedule which represents a tournament.
/// \details It contains a list of all the matches which take place during the tournament.
/// \details Added to that it also contains a list of all the matches on the specific day of the tournament.
public class Schedule 
{
    public ScheduleTypes ScheduleID { get; }
    public List<Match> Matches { get; }

    public Schedule(string PathToCsvFile, SportsTypes sport_type, ScheduleTypes schedule_type)
    {
        this.ScheduleID = schedule_type;
        this.Matches = GetMatchesFromCsvFile(PathToCsvFile, sport_type);
    }

    public List<Match> GetMatchesFromCsvFile(string PathToCsvFile, SportsTypes sport_type)
    {
        return CSVReader<Match>.GetScheduleFromCsvFile(PathToCsvFile, sport_type);
    }

    public List<Match> GetMatchesOnDay()
    {
        List<Match> MatchesOnDay = new List<Match>();
        foreach(var match in Matches)
        {
            if(DateTime.Today == match.MatchDate.Date)
            {
                MatchesOnDay.Add(match);
            }
        }
        return MatchesOnDay;
    }
}
