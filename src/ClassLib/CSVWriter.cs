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

    public static void TrackScoreData(
        string PathToCsvFile,
        List<Member<P, M>> members,
        PredictionGame prediction_game
    )
    {
        if (prediction_game.ScheduleTypes.Count == 0)
        {
            throw new InvalidOperationException("PredictionGame.ScheduleTypes is not initialized.");
        }

        using (StreamWriter sw = new StreamWriter(PathToCsvFile))
        {
            int AmountOfScheduleTypes = prediction_game.ScheduleTypes.Count;
            string[] PredictableSchedules = new string[AmountOfScheduleTypes];
            for (int i = 0; i < AmountOfScheduleTypes; i++)
            {
                PredictableSchedules[i] = $";{prediction_game.ScheduleTypes[i]}";
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

    public static void TrackFootballPredictionData(
        string PathToCsvFile,
        List<FootballPrediction> PredictionsToArchive,
        PredictionGame prediction_game
    )
    {
        if (prediction_game.Members.Count == 0)
        {
            throw new InvalidOperationException(
                "There are no members to track prediction data for."
            );
        }

        using (StreamWriter sw = new StreamWriter(PathToCsvFile))
        {
            int member_count = prediction_game.Members.Count;
            string[] member_ids = new string[member_count];
            for (int i = 0; i < member_count; i++)
            {
                member_ids[i] = $";{prediction_game.Members[i].MemberID}";
            }
            sw.WriteLine($"MemberID{string.Join("", member_ids)}");

            foreach (var prediction in PredictionsToArchive)
            {
                string prediced_match_data =
                    $"{prediction.HomeTeam}" + " : " + $"{prediction.AwayTeam}";
                foreach (var member in prediction_game.Members)
                {
                    string member_prediction =
                        $"{prediction.PredictionHome}" + " : " + $"{prediction.PredictionAway}";

                    sw.WriteLine(prediced_match_data + member_prediction);
                }
            }
        }
    }
}
