using System;
using System.Collections.Generic;

/// \brief Generic class Schedule which represents a tournament.
/// \details It contains a list of all the matches which take place during the tournament.
/// \details Added to that it also contains a list of all the matches on the specific day of the tournament.
public class Schedule<M>
    where M : Match
{
    public ScheduleTypes ScheduleID { get; }
    public List<M> Matches { get; }
    public List<M> MatchesOnDay { get; }

    public Schedule(ScheduleTypes schedule_type, string PathToCsvFile)
    {
        // code
    }

    List<M> GetMatchesFromCsvFile(string PathToCsvFile, ScheduleTypes schedule_type)
    {
        // code
    }

    List<M> GetMatchesOnDay(DateTime date)
    {
        // code
    }
}
