using System;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;

public class CSVWriter<M, P>
    where M : Match
    where P : Prediction
{
    public static void UpdateSchedule(string PathToCsvFile, List<M?> schedule)
    {
        using (StreamWriter sw = new StreamWriter(PathToCsvFile))
        {
            sw.WriteLine(
                "MatchID;Date;Home Team;Away Team;Result Home Team;Result Away Team;Result Home Team Penalties;Result Away Team Penalties" //header row
            );
            foreach (var m in schedule)
            {
                sw.WriteLine(m?.ToString()); //writes the match data for every match of the schedule
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
            // Kopfzeile der CSV-Datei anpassen
            sw.WriteLine("MemberID;Forename;Surname;Email Address;Password;ParticipatingSchedules");

            foreach (var member in prediction_game.Members)
            {
                // Member-Daten in eine Zeile schreiben
                string memberLine =
                    $"{member.MemberID};{member.GetForename()};{member.GetSurname()};{member.GetEmailAddress()};{member.GetPassword()}";

                // Schedule-Teilnahmen des Mitglieds als Liste von ScheduleTypes
                var participatingSchedules =
                    member.GetParticipatingSchedules()?.Select(s => s?.ScheduleID.ToString())
                    ?? new List<string>();
                string schedulesString = string.Join(",", participatingSchedules);

                // Gesamtzeile schreiben
                sw.WriteLine($"{memberLine};{schedulesString}");
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
                PredictableSchedules[i] = $"{prediction_game.ScheduleTypesList[i]}";
            }
            sw.WriteLine($"MemberID;{string.Join(";", PredictableSchedules)}");

            foreach (var member in prediction_game.Members)
            {
                var memberScores = member.GetScores();
                var scoresDict = memberScores.ToDictionary(
                    score => score.ScoreID,
                    score => score.ToString()
                );

                string[] Scores = new string[AmountOfScheduleTypes];

                for (int i = 0; i < AmountOfScheduleTypes; i++)
                {
                    var scheduleType = prediction_game.ScheduleTypesList[i];
                    Scores[i] = scoresDict.TryGetValue(scheduleType, out var score) ? score : "0";
                }

                sw.WriteLine($"{member.MemberID};{string.Join(";", Scores)}");
            }
        }
    }

    public static void TrackFootballPredictionData(
        string PathToCsvFile,
        PredictionGame? prediction_game
    )
    {
        if (prediction_game?.Members.Count == 0)
        {
            throw new InvalidOperationException(
                "There are no members to track prediction data for."
            );
        }

        if (prediction_game != null)
        {
            using (StreamWriter sw = new StreamWriter(PathToCsvFile))
            {
                int member_count = prediction_game.Members.Count;
                string[] member_ids = new string[member_count];
                for (int i = 0; i < member_count; i++)
                {
                    member_ids[i] = $";{prediction_game.Members[i].MemberID}";
                }
                sw.WriteLine(
                    $"Predicted Match{string.Join("", member_ids)};MatchData;PredictionDate;PredictionID"
                );

                var predicted_matches = new HashSet<FootballMatch>();
                foreach (var member in prediction_game.Members)
                {
                    foreach (var prediction in member.GetArchivedPredictions())
                    {
                        if (prediction?.PredictedMatch is FootballMatch football_match)
                        {
                            predicted_matches.Add(football_match);
                        }
                    }
                    foreach (var prediction in member.GetPredictionsDone())
                    {
                        if (prediction?.PredictedMatch is FootballMatch football_match)
                        {
                            predicted_matches.Add(football_match);
                        }
                    }
                }

                foreach (var match in predicted_matches)
                {
                    var row = new List<string> { $"{match.HomeTeam} - {match.AwayTeam}" };
                    DateTime? prediction_date = null;
                    uint? prediction_id = null;

                    foreach (var member in prediction_game.Members)
                    {
                        bool CalculateScoreAlreadyDone = false; // Reset for each member
                        FootballPrediction? footballPrediction = null;

                        var prediction = member
                            .GetArchivedPredictions()
                            .FirstOrDefault(p => p?.PredictedMatch == match);
                        if (prediction is FootballPrediction archivedFootballPrediction)
                        {
                            CalculateScoreAlreadyDone = true;
                            prediction_date = archivedFootballPrediction.PredictionDate;
                            prediction_id = archivedFootballPrediction.PredictionID;
                            footballPrediction = archivedFootballPrediction;
                        }
                        else
                        {
                            prediction = member
                                .GetPredictionsDone()
                                .FirstOrDefault(p => p?.PredictedMatch == match);
                            if (prediction is FootballPrediction doneFootballPrediction)
                            {
                                prediction_date = doneFootballPrediction.PredictionDate;
                                prediction_id = doneFootballPrediction.PredictionID;
                                footballPrediction = doneFootballPrediction;
                            }
                        }

                        if (footballPrediction != null)
                        {
                            row.Add(
                                $"{footballPrediction.PredictionHome}:{footballPrediction.PredictionAway}:{(CalculateScoreAlreadyDone ? 1 : 0)}"
                            );
                        }
                        else
                        {
                            row.Add("");
                        }
                    }
                    row.Add($"{match};{prediction_date?.ToString() ?? "N/A"};{prediction_id}");
                    sw.WriteLine(string.Join(";", row));
                }
            }
        }
    }
}
