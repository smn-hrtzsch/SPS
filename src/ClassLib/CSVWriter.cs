using System;
using System.Formats.Asn1;
using System.IO;

public class CSVWriter<M, P>
    where M : Match
    where P : Prediction
{
    public static void UpdateSchedule(string PathToCsvFile, List<M> schedule)
    {
        using (StreamWriter sw = new StreamWriter(PathToCsvFile))
        {
            sw.WriteLine(
                "MatchID;Date;Home Team;Away Team;Result Home Team;Result Away Team;Result Home Team Penalties;Result Away Team Penalties" //header row
            );
            foreach (M m in schedule)
            {
                sw.WriteLine(m.ToString()); //writes the match data for every match of the schedule
            }
        }
    }

    public static void DeleteFile(string PathToCsvFile)
    {
        File.Delete(PathToCsvFile);
    }

    public static void WriteMemberData(string PathToCsvFile, List<Member<P, M>> members)
    {
        using (StreamWriter sw = new StreamWriter(PathToCsvFile))
        {
            sw.WriteLine("MemberID;Forename;Surname;Email Address;Password");
            foreach (var member in members)
            {
                sw.WriteLine(member.ToString());
            }
        }
    }

    public static void TrackScoreData(string PathToCsvFile, List<Member<P, M>> members)
    {
        if (PredictionGame.ScheduleTypes == null)
        {
            throw new InvalidOperationException("PredictionGame.ScheduleTypes is not initialized.");
        }

        using (StreamWriter sw = new StreamWriter(PathToCsvFile))
        {
            int AmountOfScheduleTypes = PredictionGame.ScheduleTypes.Count;
            string[] PredictableSchedules = new string[AmountOfScheduleTypes];
            for (int i = 0; i < AmountOfScheduleTypes; i++)
            {
                PredictableSchedules[i] = $";{PredictionGame.ScheduleTypes[i]}";
            }
            sw.WriteLine($"MemberID{string.Join("", PredictableSchedules)}");

            foreach (var member in members)
            {
                int AmountOfScores = member.GetScores.Count;
                string[] Scores = new string[AmountOfScores];
                for (int i = 0; i < AmountOfScores; i++)
                {
                    Scores[i] = $";{member.GetScores[i].ToString()}";
                }
                sw.WriteLine($"{member.MemberID}{string.Join("", Scores)}");
            }
        }
    }
}
