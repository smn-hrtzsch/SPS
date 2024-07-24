using System;
using System.Collections.Generic;
using System.IO;

public class Program
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

        // Main menu loop
        while (true)
        {
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("              SPS              ");
            Console.WriteLine("================================");
            Console.WriteLine("[1] Display Members");
            Console.WriteLine("[2] Display Scores");
            Console.WriteLine("[3] Add Prediction");
            Console.WriteLine("[4] Save and Exit");
            Console.Write("Select an option (1-4): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayMembers(prediction_game);
                    break;
                case "2":
                    DisplayScores(prediction_game);
                    break;
                case "3":
                    AddPrediction(prediction_game, em_2024);
                    break;
                case "4":
                    SaveAndExit(
                        prediction_game,
                        PathToMemberDataFile,
                        PathToPredictionDataFile,
                        PathToScoreDataFile
                    );
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void DisplayMembers(PredictionGame prediction_game)
    {
        Console.Clear();
        Console.WriteLine("Members:");
        foreach (var member in prediction_game.Members)
        {
            Console.WriteLine($"{member.MemberID}: {member.GetForename()} {member.GetSurname()}");
        }
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }

    private static void DisplayScores(PredictionGame prediction_game)
    {
        Console.Clear();
        Console.WriteLine("Scores:");
        foreach (var member in prediction_game.Members)
        {
            Console.WriteLine($"{member.MemberID}: {member.GetForename()} {member.GetSurname()}");
            foreach (var score in member.GetScores())
            {
                Console.WriteLine($"\t{score.ScoreID}: {score.AmountOfPoints} points");
            }
        }
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }

    private static void AddPrediction(PredictionGame prediction_game, Schedule<Match> em_2024)
    {
        Console.Clear();
        Console.WriteLine("Add Prediction:");
        Console.Write("Enter Member ID: ");
        if (uint.TryParse(Console.ReadLine(), out uint memberId))
        {
            var member = prediction_game.Members.Find(m => m.MemberID == memberId);
            if (member == null)
            {
                Console.WriteLine("Member not found.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Available Matches:");
            foreach (var match in em_2024.Matches)
            {
                Console.WriteLine($"{match.MatchID}: {match.ToString()}");
            }

            Console.Write("Enter Match ID: ");
            if (uint.TryParse(Console.ReadLine(), out uint matchId))
            {
                var match = em_2024.Matches.Find(m => m.MatchID == matchId);
                if (match == null)
                {
                    Console.WriteLine("Match not found.");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Enter Prediction for Home Team: ");
                if (byte.TryParse(Console.ReadLine(), out byte predictionHome))
                {
                    Console.Write("Enter Prediction for Away Team: ");
                    if (byte.TryParse(Console.ReadLine(), out byte predictionAway))
                    {
                        //member.AddPrediction(new FootballPrediction(memberId, match, DateTime.Now, predictionHome, predictionAway));
                        Console.WriteLine("Prediction added successfully!");
                    }
                }
            }
        }

        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }

    private static void SaveAndExit(
        PredictionGame prediction_game,
        string memberFilePath,
        string predictionFilePath,
        string scoreFilePath
    )
    {
        CSVWriter<Match, Prediction>.WriteMemberData(memberFilePath, prediction_game);
        CSVWriter<Match, Prediction>.TrackFootballPredictionData(
            predictionFilePath,
            prediction_game
        );
        CSVWriter<Match, Prediction>.TrackScoreData(scoreFilePath, prediction_game);

        Console.WriteLine("Data saved successfully. Exiting...");
        Console.WriteLine("Press any key to exit the program.");
        Console.ReadKey();
    }
}


// // Member<Match, Prediction> simon = new Member<Match, Prediction>(
// //     "Simon",
// //     "Hörtzsch",
// //     "Simon.Hoertzsch@student.tu-freiberg.de",
// //     "MeinCoolesPasswort"
// // );
// // Member<Match, Prediction> artim = new Member<Match, Prediction>(
// //     "Artim",
// //     "Meyer",
// //     "Artim.Meyer@student.tu-freiberg.de",
// //     "MeinPasswortIstCooler"
// // );
// // Member<Match, Prediction> zug = new Member<Match, Prediction>(
// //     "Sebastian",
// //     "Zug",
// //     "Sebastian.Zug@informatik.tu-freiberg.de",
// //     "Ich bin Prof"
// // );

// foreach (var member in prediction_game.Members)
// {
//     Console.WriteLine($"{member}");
//     Console.WriteLine($"Liste ArchivedPredicitons for {member.GetForename()}:");
//     foreach (var prediction in member.GetArchivedPredictions())
//     {
//         Console.WriteLine($"\t{prediction}");
//     }
//     Console.WriteLine($"Liste PredictionsDone for {member.GetForename()}:");
//     foreach (var prediction in member.GetPredictionsDone())
//     {
//         Console.WriteLine($"\t{prediction}");
//     }
//     Console.WriteLine($"Liste Scores for {member.GetForename()}:");
//     foreach (var score in member.GetScores())
//     {
//         Console.WriteLine($"\t{score}");
//     }
// }

// // prediction_game.Register(simon);
// // prediction_game.Register(artim);
// // prediction_game.Register(zug);

// foreach (var member in prediction_game.Members)
// {
//     member.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);
//     member.AddPredictionToDo();
// }

// // simon.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);
// // artim.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);

// // simon.AddPredictionToDo();
// // artim.AddPredictionToDo();

// prediction_game
//     .Members[0]
//     .ConvertPredictionsDone(
//         prediction_game.Members[0].GetPredictionsToDo()[0].MatchID,
//         3,
//         2
//     );
// prediction_game
//     .Members[1]
//     .ConvertPredictionsDone(
//         prediction_game.Members[1].GetPredictionsToDo()[0].MatchID,
//         1,
//         2
//     );
// prediction_game
//     .Members[2]
//     .ConvertPredictionsDone(
//         prediction_game.Members[2].GetPredictionsToDo()[0].MatchID,
//         1,
//         1
//     );

// // simon.CalculateScores();
// // artim.CalculateScores();

// prediction_game
//     .Members[0]
//     .ConvertPredictionsDone(
//         prediction_game.Members[0].GetPredictionsToDo()[0].MatchID,
//         2,
//         3
//     );
// prediction_game
//     .Members[1]
//     .ConvertPredictionsDone(
//         prediction_game.Members[1].GetPredictionsToDo()[0].MatchID,
//         2,
//         1
//     );
// prediction_game
//     .Members[2]
//     .ConvertPredictionsDone(
//         prediction_game.Members[2].GetPredictionsToDo()[0].MatchID,
//         1,
//         1
//     );

// foreach (var member in prediction_game.Members)
// {
//     member.CalculateScores();
// }
