using System;
using System.Formats.Asn1;
using System.IO;

public class CSVWriter<M>
    where M : Match
{
    public static void UpdateSchedule(string PathToCsvFile, List<Match> schedule)
    {
        using (StreamWriter sw = new StreamWriter(PathToCsvFile))
        {
            sw.WriteLine(
                "MatchID;Date;Home Team;Away Team;Result Home Team;Result Away Team;Result Home Team Penalties;Result Away Team Penalties" //header row
            );
            foreach (Match m in schedule)
            {
                sw.WriteLine(m.ToString()); //writes the match data for every match of the schedule
            }
        }
    }

    public static void DeleteScheduleFile(string PathToCsvFile)
    {
        File.Delete(PathToCsvFile);
    }
}
