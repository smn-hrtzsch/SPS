using System;
using System.Formats.Asn1;
using System.Globalization;
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

    public static void WriteMemberData(string PathToCsvFile, PredictionGame prediction_game)
    {
        using (StreamWriter sw = new StreamWriter(PathToCsvFile))
        {
            sw.WriteLine("MemberID;Forename;Surname;Email Address;Password");
            foreach (var member in prediction_game.Members)
            {
                sw.WriteLine(member.ToString());
            }
        }
    }

    public static void TrackScoreData(string PathToCsvFile, PredictionGame prediction_game)
    {
        if (prediction_game.ScheduleTypesList.Count == 0)
        {
            throw new InvalidOperationException("PredictionGame.ScheduleTypes is not initialized.");
        }

        using (StreamWriter sw = new StreamWriter(PathToCsvFile))
        {
            int AmountOfScheduleTypes = prediction_game.ScheduleTypesList.Count;
            string[] PredictableSchedules = new string[AmountOfScheduleTypes];
            for (int i = 0; i < AmountOfScheduleTypes; i++)
            {
                PredictableSchedules[i] = $";{prediction_game.ScheduleTypesList[i]}";
            }
            sw.WriteLine($"MemberID{string.Join("", PredictableSchedules)}");

            foreach (var member in prediction_game.Members)
            {
                int AmountOfScores = member.GetScores().Count;
                string[] Scores = new string[AmountOfScores];
                for (int i = 0; i < AmountOfScores; i++)
                {
                    Scores[i] = $";{member.GetScores()[i].ToString()}";
                }
                sw.WriteLine($"{member.MemberID}{string.Join("", Scores)}");
            }
        }
    }

    public static void TrackFootballPredictionData(
        string PathToCsvFile,
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
            sw.WriteLine(
                $"Predicted Match{string.Join("", member_ids)};CalculateScore() already DONE;MatchData;PredictionDate"
            );

            var predicted_matches = new HashSet<FootballMatch>();
            foreach (var member in prediction_game.Members)
            {
                foreach (var prediction in member.GetArchivedPredictions())
                {
                    if (prediction.PredictedMatch is FootballMatch football_match)
                    {
                        predicted_matches.Add(football_match);
                    }
                }
                foreach (var prediction in member.GetPredictionsDone())
                {
                    if (prediction.PredictedMatch is FootballMatch football_match)
                    {
                        predicted_matches.Add(football_match);
                    }
                }
            }

            foreach (var match in predicted_matches)
            {
                var row = new List<string> { $"{match.HomeTeam} - {match.AwayTeam}" };
                bool CalculateScoreAlreadyDone = false;
                DateTime? prediction_date = null;

                foreach (var member in prediction_game.Members)
                {
                    FootballPrediction footballPrediction = null;
                    var prediction = member
                        .GetArchivedPredictions()
                        .FirstOrDefault(p => p.PredictedMatch == match);
                    if (prediction is FootballPrediction archivedFootballPrediction)
                    {
                        CalculateScoreAlreadyDone = true;
                        prediction_date = archivedFootballPrediction.PredictionDate;
                        footballPrediction = archivedFootballPrediction;
                    }
                    else
                    {
                        prediction = member
                            .GetPredictionsDone()
                            .FirstOrDefault(p => p.PredictedMatch == match);
                        if (prediction is FootballPrediction doneFootballPrediction)
                        {
                            prediction_date = doneFootballPrediction.PredictionDate;
                            footballPrediction = doneFootballPrediction;
                        }
                    }

                    if (footballPrediction != null)
                    {
                        row.Add(
                            $"{footballPrediction.PredictionHome}:{footballPrediction.PredictionAway}"
                        );
                    }
                    else
                    {
                        row.Add("");
                    }
                }

                if (CalculateScoreAlreadyDone)
                {
                    row.Add($"1;{match};{prediction_date?.ToString() ?? "N/A"}");
                }
                else
                {
                    row.Add($"0;{match};{prediction_date?.ToString() ?? "N/A"}");
                }
                sw.WriteLine(string.Join(";", row));
            }
        }
    }
}
