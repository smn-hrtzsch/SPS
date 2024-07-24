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
            // Anmeldescreen anzeigen
            Console.WriteLine("================================");
            Console.WriteLine("          Login Screen          ");
            Console.WriteLine("================================");
            Console.WriteLine("");
            Console.WriteLine("[1] Login");
            Console.WriteLine("[2] Register");
            Console.WriteLine("");
            Console.Write("Please choose [1] or [2]: ");
            string input = Console.ReadLine();
            
            if (input == "1")
            {
                while (true)
                {
                    // Benutzername abfragen
                    Console.Write("Enter your [Email]: ");
                    string email = Console.ReadLine();
                    bool emailFound = false;

                    foreach (var member in prediction_game.Members)
                    {
                        if (member.GetEmailAddress() == email)
                        {
                            emailFound = true;
                            Console.WriteLine("");
                            Console.Write("Now enter your password: ");
                            string password = Console.ReadLine();

                            if (member.GetPassword() == password)
                            {
                                Console.WriteLine("Login successful!");
                                // Weiter mit dem Programm nach erfolgreichem Login
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Incorrect password. Please try again.");
                                Thread.Sleep(1000);
                            }
                        }
                    }

                    if (!emailFound)
                    {
                        Console.WriteLine("Email not found. Please try again.");
                        Thread.Sleep(1000);
                    }
                }
            }
            else if (input == "2")
            {
                bool emailFound = false;
                Console.Write("Enter your new [Email]: ");
                string newEmail = Console.ReadLine();

                foreach(var member in prediction_game.Members)
                {
                    if(member.GetEmailAddress() == newEmail)
                    {
                        emailFound = true;
                        Console.WriteLine("");
                        Console.WriteLine("Entered Email is already existing, try to login.");
                        Console.WriteLine("Press any key to return to the login screen...");
                        Console.ReadKey();
                        
                        
                    }
                }

                if(!emailFound)
                {
                    Console.Write("Enter your new [Forename]: ");
                    string newForename = Console.ReadLine();

                    Console.Write("Enter your new [Surname]: ");
                    string newSurname = Console.ReadLine();
                    Console.Write("Enter your new [Password]: ");
                    string newPassword = Console.ReadLine();

                    prediction_game.Members.Add(new Member<Match, Prediction>(newForename, newSurname, newEmail, newPassword));
                    Console.WriteLine("Registration successful!");
                    Thread.Sleep(1000);

                }
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("That did not work. Please try again.");
                Console.WriteLine("");
                Thread.Sleep(1000);
            }

            Console.Clear();
        }
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
