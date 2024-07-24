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
    }
}
        



        // // Benutzername abfragen
        // Console.Write("Benutzername: ");
        // string benutzername = Console.ReadLine();

        // // Passwort abfragen
        // Console.Write("Passwort: ");
        // string passwort = Console.ReadLine();

        // // Anmeldeinformationen verarbeiten (nur ein Beispiel, hier könntest du eine Validierung hinzufügen)
        // if (benutzername == "admin" && passwort == "passwort123")
        // {
        //     Console.WriteLine("\nAnmeldung erfolgreich!");
        // }
        // else
        // {
        //     Console.WriteLine("\nUngültiger Benutzername oder Passwort.");
        // }

//Simons Test

//         Member<Match, Prediction> simon = new Member<Match, Prediction>(
//             "Simon",
//             "Hörtzsch",
//             "Simon.Hoertzsch@student.tu-freiberg.de",
//             "MeinCoolesPasswort"
//         );
//         Member<Match, Prediction> artim = new Member<Match, Prediction>(
//             "Artim",
//             "Meyer",
//             "Artim.Meyer@student.tu-freiberg.de",
//             "MeinPasswortIstCooler"
//         );
//         Member<Match, Prediction> zug = new Member<Match, Prediction>(
//             "Sebastian",
//             "Zug",
//             "Sebastian.Zug@informatik.tu-freiberg.de",
//             "Ich bin Prof"
//         );

//         foreach (var member in prediction_game.Members)
//         {
//             Console.WriteLine($"Liste ArchivedPredicitons for {member.GetForename()}:");
//             foreach (var prediction in member.GetArchivedPredictions())
//             {
//                 Console.WriteLine($"\t{prediction}");
//             }
//             Console.WriteLine($"Liste PredictionsDone for {member.GetForename()}:");
//             foreach (var prediction in member.GetPredictionsDone())
//             {
//                 Console.WriteLine($"\t{prediction}");
//             }
//             Console.WriteLine($"Liste Scores for {member.GetForename()}:");
//             foreach (var score in member.GetScores())
//             {
//                 Console.WriteLine($"\t{score}");
//             }
//         }

//         // prediction_game.Register(simon);
//         // prediction_game.Register(artim);
//         // prediction_game.Register(zug);

//         foreach (var member in prediction_game.Members)
//         {
//             member.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);
//             member.AddPredictionToDo();
//         }

//         // simon.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);
//         // artim.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);

//         // simon.AddPredictionToDo();
//         // artim.AddPredictionToDo();

//         prediction_game
//             .Members[0]
//             .ConvertPredictionsDone(
//                 prediction_game.Members[0].GetPredictionsToDo()[0].MatchID,
//                 3,
//                 2
//             );
//         prediction_game
//             .Members[1]
//             .ConvertPredictionsDone(
//                 prediction_game.Members[1].GetPredictionsToDo()[0].MatchID,
//                 1,
//                 2
//             );
//         prediction_game
//             .Members[2]
//             .ConvertPredictionsDone(
//                 prediction_game.Members[2].GetPredictionsToDo()[0].MatchID,
//                 1,
//                 1
//             );

//         // simon.CalculateScores();
//         // artim.CalculateScores();

//         prediction_game
//             .Members[0]
//             .ConvertPredictionsDone(
//                 prediction_game.Members[0].GetPredictionsToDo()[0].MatchID,
//                 2,
//                 3
//             );
//         prediction_game
//             .Members[1]
//             .ConvertPredictionsDone(
//                 prediction_game.Members[1].GetPredictionsToDo()[0].MatchID,
//                 2,
//                 1
//             );
//         prediction_game
//             .Members[2]
//             .ConvertPredictionsDone(
//                 prediction_game.Members[2].GetPredictionsToDo()[0].MatchID,
//                 1,
//                 1
//             );

//         foreach (var member in prediction_game.Members)
//         {
//             member.CalculateScores();
//         }

//         CSVWriter<Match, Prediction>.WriteMemberData(PathToMemberDataFile, prediction_game);
//         CSVWriter<Match, Prediction>.TrackFootballPredictionData(
//             PathToPredictionDataFile,
//             prediction_game
//         );
//         CSVWriter<Match, Prediction>.TrackScoreData(PathToScoreDataFile, prediction_game);

//         Console.WriteLine("Erverything should work :) (pllllsssssss!!!!!!!!)");
//     }
// }
