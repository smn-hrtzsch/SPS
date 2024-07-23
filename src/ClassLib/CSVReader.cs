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

    public List<Member<M, P>> GetMemberDataFromCsvFile(string PathToCsvFile)
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

    public void GetScoresFromCsvFile(string PathToCsvFile, PredictionGame prediction_game)
    {
        List<Score> scores = new List<Score>();

        var all_lines = File.ReadLines(PathToCsvFile).ToList();

        int AmountOfScheduleTypes = prediction_game.ScheduleTypesList.Count;

        for (int line_number = 1; line_number < all_lines.Count; line_number++)
        {
            string? needed_line = all_lines[line_number];
            string[] score_data = needed_line.Split(";");
            for (int score_number = 1; score_number < AmountOfScheduleTypes; score_number++)
            {
                ScheduleTypes schedule_type = (ScheduleTypes)
                    Enum.Parse(typeof(ScheduleTypes), score_data[score_number]);
                uint amount_of_points = uint.Parse(score_data[1]);
                scores.Add(new Score(schedule_type, amount_of_points));
            }

            prediction_game.Members[line_number - 1].SetScores(scores);
        }
    }

    public void GetFootballPredictionsFromCsvFile(
        string PathToCsvFile,
        PredictionGame prediction_game,
        Schedule<FootballMatch> schedule
    )
    {
        List<Prediction> done_predictions = new List<Prediction>();
        List<Prediction> archived_predictions = new List<Prediction>();

        var all_lines = File.ReadLines(PathToCsvFile).ToList();

        int AmountOfScheduleTypes = prediction_game.ScheduleTypesList.Count;

        for (int line_number = 1; line_number < all_lines.Count; line_number++)
        {
            string needed_line = all_lines[line_number];
            string[] prediction_data = needed_line.Split(';');

            FootballMatch? football_match = schedule.Matches.FirstOrDefault(m =>
                m.ToString() == prediction_data[prediction_game.Members.Count + 2]
            );

            foreach (var member in prediction_game.Members)
            {
                int member_count = prediction_game.Members.Count;
                string[] member_predictions = new string[member_count];

                for (int i = 1; i < member_count; i++)
                {
                    member_predictions[i] = prediction_data[i];
                    string[] prediction_bytes = member_predictions[i].Split(':');
                    byte prediction_home = byte.Parse(prediction_bytes[0]);
                    byte prediction_away = byte.Parse(prediction_bytes[1]);
                    DateTime prediction_date = DateTime.ParseExact(
                        prediction_data[prediction_game.Members.Count + 3],
                        "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture
                    );
                    FootballPrediction football_prediction = new FootballPrediction(
                        member.MemberID,
                        football_match,
                        DateTime.Now,
                        prediction_home,
                        prediction_away
                    );
                    byte CalculateScoreAlreadyDone = byte.Parse(
                        prediction_data[prediction_game.Members.Count + 1]
                    );

                    if (CalculateScoreAlreadyDone == 1)
                    {
                        archived_predictions.Add(football_prediction);
                    }
                    else
                    {
                        done_predictions.Add(football_prediction);
                    }
                }
                member.SetArchivedPredictions(archived_predictions);
                member.SetPredictionsDone(done_predictions);
            }
        }
    }
}
