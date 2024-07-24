using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

class Program
{
    public static void Main(string[] args)
    {
        // Set up email service and initialize prediction game
        EmailService emailService = new EmailService();
        PredictionGame prediction_game = new PredictionGame(emailService);

        // Set up match factory for the CSVReader
        IMatchFactory<FootballMatch> football_match_factory = new FootballMatchFactory();
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(football_match_factory);

        // File paths
        string PathToScheduleFile = "../../csv-files/EM_2024.csv";
        string PathToMemberDataFile = "../../csv-files/MemberData.csv";
        string PathToPredictionDataFile = "../../csv-files/PredictionData.csv";
        string PathToScoreDataFile = "../../csv-files/ScoreData.csv";

        // Load schedule, members, predictions, and scores
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
        // //Gerneral Programm variable declaration
        // EmailService emailService = new EmailService();
        // PredictionGame prediction_game = new PredictionGame(
        //     emailService);
        // string PathToMemberDataFile = "../../csv-files/MemberData.csv";

        // if (File.Exists(PathToMemberDataFile))
        // {
        //     prediction_game.Members = CSVReader<Match, Prediction>.GetMemberDataFromCsvFile(
        //         PathToMemberDataFile
        //     );
        // }

        foreach (var member in prediction_game.Members)
        {
            member.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);
            member.AddPredictionToDo();
        }

        //Email continuous Integration

        prediction_game.SendDailyEmail();
        //Email continous Integration end
    }
}


// //Email continuous Integration

//         DateTime dateTimeNow = DateTime.Now;
//         DateTime dateTimeAtNineThirty = new DateTime(
//             dateTimeNow.Year,
//             dateTimeNow.Month,
//             dateTimeNow.Day,
//             9,
//             30,
//             0
//         );
//         DateTime dateTimeAtEighteenOClock = new DateTime(
//             dateTimeNow.Year,
//             dateTimeNow.Month,
//             dateTimeNow.Day,
//             18,
//             0,
//             0
//         );

//         if (dateTimeNow == dateTimeAtNineThirty) //get daily Tipp-email
//         {
//             predictionGame.SendDailyEmail();
//         }

//         if (dateTimeNow.DayOfWeek == DayOfWeek.Sunday && dateTimeNow == dateTimeAtEighteenOClock) //get Results-email once a week
//         {
//             predictionGame.SendDailyEmail();
//         }

//         //Email continous Integration end
