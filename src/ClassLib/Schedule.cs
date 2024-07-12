using System;
using System.Collections.Generic;

public class Schedule<T>
    where T : Match
{
    public ScheduleTypes ScheduleID { get; }
    public List<T> Matches { get; }
    public List<T> MatchesOnDay { get; }

    public Schedule(SchedulTypes schedule_type, string PathToCsvFile)
    {
        // code
    }

    List<Match> GetMatchesFromCsvFile(string PathToCsvFile, ScheduleTypes schedule_type)
    {
        // code
    }

    List<Match> GetMatchesOnDay(DateTime date)
    {
        // code
    }
}
