using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

class Program
{
    public static void Main(string[] args)
    {
        //General Program variable declaration
        // set up email service
        EmailService emailService = new EmailService();

        // initialize prediction_game
        PredictionGame prediction_game = new PredictionGame(emailService);

        // set MatchFactory for the CSVReader
        IMatchFactory<FootballMatch> football_match_factory = new FootballMatchFactory();
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(football_match_factory);

        // set up the paths to the required csv files
        string PathToScheduleFile = "../../csv-files/EM_2024.csv";
        string PathToMemberDataFile = "../../csv-files/MemberData.csv";
        string PathToPredictionDataFile = "../../csv-files/PredictionData.csv";
        string PathToScoreDataFile = "../../csv-files/ScoreData.csv";

        // set up schedule for EM 2024
        Schedule<Match> em_2024;

        if (File.Exists(PathToScheduleFile))
        {
            em_2024 = new Schedule<Match>(
                PathToScheduleFile,
                SportsTypes.Football,
                ScheduleTypes.EM_2024
            );
            if (File.Exists(PathToMemberDataFile))
            {
                prediction_game.Members = CSVReader<Match, Prediction>.GetMemberDataFromCsvFile(
                    PathToMemberDataFile
                );
            }
            if (File.Exists(PathToPredictionDataFile))
            {
                CSVReader<Match, Prediction>.GetFootballPredictionsFromCsvFile(
                    PathToPredictionDataFile,
                    prediction_game,
                    em_2024
                );
            }
            if (File.Exists(PathToScoreDataFile))
            {
                CSVReader<Match, Prediction>.GetScoresFromCsvFile(
                    PathToScoreDataFile,
                    prediction_game
                );
            }
        }
        else
        {
            throw new InvalidOperationException("There is no file to read the schedule from.");
        }

        Member<Match, Prediction> simon = new Member<Match, Prediction>(
            "Simon",
            "Hörtzsch",
            "Simon.Hoertzsch@student.tu-freiberg.de",
            "MeinCoolesPasswort"
        );
        Member<Match, Prediction> artim = new Member<Match, Prediction>(
            "Artim",
            "Meyer",
            "Artim.Meyer@student.tu-freiberg.de",
            "MeinPasswortIstCooler"
        );
        Member<Match, Prediction> zug = new Member<Match, Prediction>(
            "Sebastian",
            "Zug",
            "Sebastian.Zug@informatik.tu-freiberg.de",
            "Ich bin Prof"
        );

        foreach (var member in prediction_game.Members)
        {
            Console.WriteLine($"Liste ArchivedPredicitons for {member.GetForename()}:");
            foreach (var prediction in member.GetArchivedPredictions())
            {
                Console.WriteLine($"\t{prediction}");
            }
            Console.WriteLine($"Liste PredictionsDone for {member.GetForename()}:");
            foreach (var prediction in member.GetPredictionsDone())
            {
                Console.WriteLine($"\t{prediction}");
            }
            Console.WriteLine($"Liste Scores for {member.GetForename()}:");
            foreach (var score in member.GetScores())
            {
                Console.WriteLine($"\t{score}");
            }
        }

        // prediction_game.Register(simon);
        // prediction_game.Register(artim);
        // prediction_game.Register(zug);

        foreach (var member in prediction_game.Members)
        {
            member.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);
            member.AddPredictionToDo();
        }

        // simon.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);
        // artim.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);

        // simon.AddPredictionToDo();
        // artim.AddPredictionToDo();

        prediction_game
            .Members[0]
            .ConvertPredictionsDone(
                prediction_game.Members[0].GetPredictionsToDo()[0].MatchID,
                3,
                2
            );
        prediction_game
            .Members[1]
            .ConvertPredictionsDone(
                prediction_game.Members[1].GetPredictionsToDo()[0].MatchID,
                1,
                2
            );
        prediction_game
            .Members[2]
            .ConvertPredictionsDone(
                prediction_game.Members[2].GetPredictionsToDo()[0].MatchID,
                1,
                1
            );

        // simon.CalculateScores();
        // artim.CalculateScores();

        prediction_game
            .Members[0]
            .ConvertPredictionsDone(
                prediction_game.Members[0].GetPredictionsToDo()[0].MatchID,
                2,
                3
            );
        prediction_game
            .Members[1]
            .ConvertPredictionsDone(
                prediction_game.Members[1].GetPredictionsToDo()[0].MatchID,
                2,
                1
            );
        prediction_game
            .Members[2]
            .ConvertPredictionsDone(
                prediction_game.Members[2].GetPredictionsToDo()[0].MatchID,
                1,
                1
            );

        foreach (var member in prediction_game.Members)
        {
            member.CalculateScores();
        }

        CSVWriter<Match, Prediction>.WriteMemberData(PathToMemberDataFile, prediction_game);
        CSVWriter<Match, Prediction>.TrackFootballPredictionData(
            PathToPredictionDataFile,
            prediction_game
        );
        CSVWriter<Match, Prediction>.TrackScoreData(PathToScoreDataFile, prediction_game);

        Console.WriteLine("Erverything should work :) (pllllsssssss!!!!!!!!)");
    }
}
