using System;
using System.Collections.Generic;

/// \brief Generic class Schedule which represents a tournament.
/// \details It contains a list of all the matches which take place during the tournament.
/// \details Added to that it also contains a list of all the matches on the specific day of the tournament.
public class Schedule<T>
    where T : Match
{
    public ScheduleTypes ScheduleID { get; }
    public List<T> Matches { get; }
    public List<T> MatchesOnDay { get; }

    public Schedule(ScheduleTypes schedule_type, string PathToCsvFile)
    {
        // code
    }

    List<T> GetMatchesFromCsvFile(string PathToCsvFile, ScheduleTypes schedule_type)
    {
        // code
    }

    List<T> GetMatchesOnDay(DateTime date)
    {
        // code
    }
}
