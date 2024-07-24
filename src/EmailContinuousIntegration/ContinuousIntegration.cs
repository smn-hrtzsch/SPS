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

        // Load schedule, members, predictions, and scores
        Schedule<Match>? em_2024 = null;
        try
        {
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
        }
        else
        {
            throw new InvalidOperationException("There is no file to read the schedule from.");
        }

        foreach (var member in prediction_game.Members)
        {
            member.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);
            member.AddPredictionToDo();
        }

        prediction_game.SendDailyEmail();
                        }
        catch (FormatException ex)
            {
            
            }
    }
}

