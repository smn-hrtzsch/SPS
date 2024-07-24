using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class CSVReader<M, P>
    where M : Match
    where P : Prediction
{
    private static IMatchFactory<M> MatchFactory;

    public static void SetMatchFactory(IMatchFactory<M> match_factory)
    {
        MatchFactory = match_factory;
    }

    /// \brief Reads the match data from a CSV file.
    /// \param PathToMatchDataCsvFile The path to the CSV file containing match data.
    /// \param MatchID The unique identifier of the match.
    /// \return An array of strings containing the match data.
    public static string[] GetMatchDataFromCsvFile(string PathToMatchDataCsvFile, int line_number)
    {
        string? needed_line = File.ReadLines(PathToMatchDataCsvFile)
            .Skip(line_number)
            .FirstOrDefault();
        if (needed_line != null)
        {
            string[] FootballMatchData = needed_line.Split(';');
            return FootballMatchData;
        }
        else
        {
            throw new InvalidOperationException("Die Zeile ist leer!");
        }
    }

    public static List<M> GetScheduleFromCsvFile(string PathToCsvFile, SportsTypes sport_type)
    {
        if (MatchFactory == null)
        {
            throw new InvalidOperationException("Match factory is not set.");
        }
        List<M> schedule = new List<M>();
        var all_lines = File.ReadLines(PathToCsvFile).ToList();
        for (int line_number = 1; line_number < all_lines.Count; line_number++)
        {
            switch (sport_type)
            {
                case SportsTypes.Football:
                    M match = MatchFactory.CreateMatch(PathToCsvFile, line_number, sport_type);
                    schedule.Add(match);
                    break;
                // could be extended by serveral sport types
            }
        }
        return schedule;
    }

    public static List<Member<M, P>> GetMemberDataFromCsvFile(string PathToCsvFile)
    {
        List<Member<M, P>> members = new List<Member<M, P>>();

        var all_lines = File.ReadLines(PathToCsvFile).ToList();

        for (int line_number = 1; line_number < all_lines.Count; line_number++)
        {
            string? needed_line = all_lines[line_number];
            string[] member_data = needed_line.Split(";");
            uint member_id = uint.Parse(member_data[0]);
            members.Add(
                new Member<M, P>(
                    member_id,
                    member_data[1],
                    member_data[2],
                    member_data[3],
                    member_data[4]
                )
            );
        }
        return members;
    }

    public static void GetScoresFromCsvFile(string PathToCsvFile, PredictionGame prediction_game)
    {
        List<Score> scores = new List<Score>();

        var all_lines = File.ReadLines(PathToCsvFile).ToList();

        int AmountOfScheduleTypes = prediction_game.ScheduleTypesList.Count;

        for (int line_number = 1; line_number < all_lines.Count; line_number++)
        {
            string? needed_line = all_lines[line_number];
            string[] score_data = needed_line.Split(";");
            for (int score_number = 1; score_number <= AmountOfScheduleTypes; score_number++)
            {
                ScheduleTypes schedule_type = (ScheduleTypes)
                    Enum.Parse(typeof(ScheduleTypes), score_data[score_number]);
                uint amount_of_points = uint.Parse(score_data[1]);
                scores.Add(new Score(schedule_type, amount_of_points));
            }

            prediction_game.Members[line_number - 1].SetScores(scores);
        }
    }

    public static void GetFootballPredictionsFromCsvFile(
        string PathToCsvFile,
        PredictionGame prediction_game,
        Schedule<M> schedule
    )
    {
        var all_lines = File.ReadLines(PathToCsvFile).ToList();

        // Erster Schritt: Kopfzeile einlesen, um die Member IDs zu extrahieren
        string header = all_lines[0];
        string[] headerColumns = header.Split(';');
        int member_count = prediction_game.Members.Count; // 5 weitere Spalten: Predicted Match, CalculateScore, MatchData, PredictionDate

        // Dictionary für jeden Member, um ihre jeweiligen Predictions zu speichern
        Dictionary<uint, List<Prediction>> done_predictions_map =
            new Dictionary<uint, List<Prediction>>();
        Dictionary<uint, List<Prediction>> archived_predictions_map =
            new Dictionary<uint, List<Prediction>>();

        foreach (var member in prediction_game.Members)
        {
            done_predictions_map[member.MemberID] = new List<Prediction>();
            archived_predictions_map[member.MemberID] = new List<Prediction>();
        }

        for (int line_number = 1; line_number < all_lines.Count; line_number++)
        {
            string needed_line = all_lines[line_number];
            string[] prediction_data = needed_line.Split(';');

            // Finde das entsprechende Match in der Schedule
            FootballMatch? football_match =
                schedule.Matches.FirstOrDefault(m =>
                    m.ToString() == prediction_data[member_count + 2]
                ) as FootballMatch;

            if (football_match == null)
            {
                continue; // Wenn das Match nicht gefunden wurde, überspringe die Zeile
            }

            DateTime prediction_date = DateTime.ParseExact(
                prediction_data[member_count + 3],
                "M/d/yyyy h:mm:ss tt",
                CultureInfo.InvariantCulture
            );

            for (int i = 1; i <= member_count; i++) // Start from 1 to skip "Predicted Match" column
            {
                string memberIdStr = headerColumns[i];
                if (!uint.TryParse(memberIdStr, out uint memberId))
                {
                    continue; // Ungültige Member ID in der Kopfzeile
                }

                var member = prediction_game.Members.FirstOrDefault(m => m.MemberID == memberId);
                if (member == null)
                {
                    continue; // Member nicht gefunden
                }

                string[] prediction_bytes = prediction_data[i].Split(':');
                if (prediction_bytes.Length < 2)
                {
                    continue; // Ungültiges Vorhersagedatenformat
                }

                byte prediction_home = byte.Parse(prediction_bytes[0]);
                byte prediction_away = byte.Parse(prediction_bytes[1]);

                FootballPrediction football_prediction = new FootballPrediction(
                    member.MemberID,
                    football_match,
                    prediction_date,
                    prediction_home,
                    prediction_away
                );

                byte CalculateScoreAlreadyDone = byte.Parse(prediction_data[member_count + 1]);

                if (CalculateScoreAlreadyDone == 1)
                {
                    archived_predictions_map[memberId].Add(football_prediction);
                }
                else
                {
                    done_predictions_map[memberId].Add(football_prediction);
                }
            }
        }

        // Setze die gesammelten Predictions für jedes Mitglied
        foreach (var member in prediction_game.Members)
        {
            member.SetArchivedPredictions(archived_predictions_map[member.MemberID]);
            member.SetPredictionsDone(done_predictions_map[member.MemberID]);
        }
    }
}
