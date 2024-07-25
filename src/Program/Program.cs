using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

public class Program
{
    static uint? member_id = null;

    public static void Main(string[] args)
    {
        // Set up email service and initialize prediction game
        // Set up email service and initialize prediction game
        EmailService emailService = new EmailService();
        PredictionGame prediction_game = new PredictionGame(emailService);
        bool login = false;
        DateTime yesterday = DateTime.Now.AddDays(-1);

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
                // Console.WriteLine("PredictionDone für Member w:");
                // foreach (var predicition in prediction_game.Members[3].GetPredictionsDone())
                // {
                //     Console.WriteLine(predicition);
                // }
                // Console.WriteLine("ArchivedPredictions für Member w:");
                // foreach (var predicition in prediction_game.Members[3].GetArchivedPredictions())
                // {
                //     Console.WriteLine(predicition);
                // }
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

        // Begin login loop / Main menu loop
        while (!login)
        {
            // Show login screen
            Console.Clear();
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
                if (prediction_game.Members.Count == 0)
                {
                    Console.WriteLine(
                        "No user is registered yet, please register before trying to log in."
                    );
                    Console.WriteLine("Please press any key to return to the login screen.");
                    Console.ReadKey();
                }
                else
                {
                    while (true)
                    {
                        // Ask user for email
                        Console.Write("Enter your [Email] or press [esc] to return: ");
                        string email = string.Empty;

                        while (true)
                        {
                            var emailInput = Console.ReadKey(true);

                            if (emailInput.Key == ConsoleKey.Escape)
                            {
                                email = string.Empty;
                                break; // Return to the start screen
                            }

                            if (emailInput.Key == ConsoleKey.Enter)
                            {
                                Console.WriteLine();
                                break; // End input
                            }

                            if (emailInput.Key == ConsoleKey.Backspace && email.Length > 0)
                            {
                                email = email.Substring(0, email.Length - 1);
                                Console.Write("\b \b"); // Erase the last character in the console
                            }
                            else if (!char.IsControl(emailInput.KeyChar))
                            {
                                email += emailInput.KeyChar;
                                Console.Write(emailInput.KeyChar);
                            }
                        }

                        if (string.IsNullOrEmpty(email))
                        {
                            break;
                        }

                        bool emailFound = false;

                        foreach (var member in prediction_game.Members)
                        {
                            if (member.GetEmailAddress() == email)
                            {
                                emailFound = true;
                                string password = string.Empty;
                                Console.Write(
                                    "\nNow enter your [password] or press [esc] to return: "
                                );

                                while (true)
                                {
                                    var passwordInput = Console.ReadKey(true);

                                    if (passwordInput.Key == ConsoleKey.Escape)
                                    {
                                        password = string.Empty;
                                        email = string.Empty;
                                        break; // Return to the start screen
                                    }

                                    if (passwordInput.Key == ConsoleKey.Enter)
                                    {
                                        Console.WriteLine();
                                        break; // End input
                                    }

                                    if (
                                        passwordInput.Key == ConsoleKey.Backspace
                                        && password.Length > 0
                                    )
                                    {
                                        password = password.Substring(0, password.Length - 1);
                                        Console.Write("\b \b"); // Erase the last character in the console
                                    }
                                    else if (!char.IsControl(passwordInput.KeyChar))
                                    {
                                        password += passwordInput.KeyChar;
                                        Console.Write("*"); // Mask the password characters
                                    }
                                }

                                if (string.IsNullOrEmpty(password))
                                {
                                    break;
                                }

                                if (member.GetPassword() == password)
                                {
                                    login = true;
                                    member_id = member.MemberID;
                                    Console.WriteLine("\nLogin successful!");
                                    Thread.Sleep(1000);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("\nIncorrect password. Please try again.");
                                    Console.WriteLine(
                                        "If you want to stop logging in, press [esc]..."
                                    );
                                }
                            }
                        }

                        if (emailFound && login)
                        {
                            break;
                        }
                        else if (!emailFound)
                        {
                            Console.WriteLine("\nEmail not found. Please try again.");
                        }

                        if (emailFound && !login)
                        {
                            continue; // Continue to the next password attempt
                        }
                    }
                }
            }
            else if (input == "2")
            {
                bool emailFound = false;
                Console.Write("Enter your new [Email]: ");
                string newEmail = Console.ReadLine();

                foreach (var member in prediction_game.Members)
                {
                    if (member.GetEmailAddress() == newEmail)
                    {
                        emailFound = true;
                        Console.WriteLine("\nEntered Email is already existing, try to login.");
                        Console.WriteLine("Press any key to return to the login screen...");
                        Console.ReadKey();
                        break;
                    }
                }

                if (!emailFound)
                {
                    Console.Write("Enter your new [Forename]: ");
                    string newForename = Console.ReadLine();

                    Console.Write("Enter your new [Surname]: ");
                    string newSurname = Console.ReadLine();

                    while (true)
                    {
                        Console.Write("Enter your new [Password]: ");
                        string newPassword = Console.ReadLine();

                        Console.Write("Verify your password: ");
                        string verifiedPassword = Console.ReadLine();

                        if (newPassword == verifiedPassword)
                        {
                            prediction_game.Members.Add(
                                new Member<Match, Prediction>(
                                    newForename,
                                    newSurname,
                                    newEmail,
                                    newPassword
                                )
                            );
                            Console.WriteLine("Registration successful!");
                            Console.WriteLine(
                                "Press any key to return to the login screen, you can now login with your provided data."
                            );
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Passwords don't match, please try again...");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("That did not work. Please try again.");
                Console.WriteLine("");
                Thread.Sleep(1000);
            }
        }

        foreach (var member in prediction_game.Members)
        {
            member.AddParticipatingSchedule(em_2024, ScheduleTypes.EM_2024);
            member.CalculateScores();
        }

        while (true)
        {
            //Console.Clear();
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
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static List<Prediction> GetPredictionsFromSpecificDate(
        DateTime date,
        Member<Match, Prediction> member
    )
    {
        List<Prediction> predictions = new List<Prediction>();
        foreach (var prediction in member.GetPredictionsDone())
        {
            if (prediction.PredictionDate.Day == date.Day)
            {
                predictions.Add(prediction);
            }
        }
        return predictions;
    }

    private static void DisplayMembers(PredictionGame prediction_game)
    {
        Console.Clear();
        Console.WriteLine("Members:\n");

        // Linie über Spaltenüberschriften
        Console.WriteLine(new string('-', 55));

        // Spaltenüberschriften
        Console.WriteLine($"| {"MemberID", -15} | {"Forename", -15} | {"Surname", -15} |");

        // Trennlinie unter den Spaltenüberschriften
        Console.WriteLine(new string('-', 55));

        // Informationen der Mitglieder
        foreach (var member in prediction_game.Members)
        {
            Console.WriteLine(
                $"| {member.MemberID, -15} | {member.GetForename(), -15} | {member.GetSurname(), -15} |"
            );
        }

        Console.WriteLine(new string('-', 55));
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }

    private static void DisplayScores(PredictionGame prediction_game)
    {
        Console.Clear();
        Console.WriteLine("Scores:\n");

        // Bestimme die maximale Breite der ersten Spalte basierend auf MemberID und Forename
        int maxMemberInfoLength =
            prediction_game
                .Members.Select(m => $"{m.MemberID} ({m.GetForename()})")
                .Max(s => s.Length) + 2; // +2 to add padding on both sides

        // Dynamische Spaltenüberschriften basierend auf ScheduleTypes
        string header = $"| {"MemberID (Forename)".PadRight(maxMemberInfoLength)} ";
        int headerLength = header.Length;
        foreach (var scoreType in prediction_game.ScheduleTypesList)
        {
            header +=
                $"| {scoreType.ToString().PadLeft((10 + scoreType.ToString().Length) / 2).PadRight(10)} ";
        }
        header += $"| {"GesamtScore".PadLeft((10 + "GesamtScore".Length) / 2).PadRight(10)} |";

        // Ausgabe der Spaltenüberschriften und Trennlinien
        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        // Ausgabe der Member und Scores
        foreach (var member in prediction_game.Members)
        {
            string row = null;
            string memberInfo = $"{member.MemberID} ({member.GetForename()})";
            if (maxMemberInfoLength < headerLength)
            {
                row = $"| {memberInfo.PadRight(headerLength - 2)}";
            }
            else
            {
                row = $"| {memberInfo.PadRight(maxMemberInfoLength + 1)}";
            }
            int totalScore = 0;

            foreach (var scheduleType in prediction_game.ScheduleTypesList)
            {
                var score = member.GetScores().FirstOrDefault(s => s.ScoreID == scheduleType);
                int points = score != null ? (int)score.AmountOfPoints : 0;
                totalScore += points;
                string pointsStr = points.ToString();
                row += $"| {pointsStr.PadLeft((10 + pointsStr.Length) / 2).PadRight(10)} ";
            }

            string totalScoreStr = totalScore.ToString();
            row += $"| {totalScoreStr.PadLeft((11 + totalScoreStr.Length) / 2).PadRight(11)} |";
            Console.WriteLine(row);
        }

        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }

    private static void AddPrediction(PredictionGame prediction_game, Schedule<Match> em_2024)
    {
        Console.Clear();

        var member = prediction_game.Members.Find(m => m.MemberID == member_id);
        if (member == null)
        {
            Console.WriteLine("Member not found.");
            Console.ReadKey();
            return;
        }

        // Console.WriteLine("PredictionsToDo:");
        // foreach (var match in member.GetPredictionsToDo())
        // {
        //     Console.WriteLine($"{match.MatchID} {match}");
        // }
        // Console.WriteLine("PredictionsDone:");
        // foreach (var prediction in member.GetPredictionsDone()) {
        //     Console.WriteLine(prediction);
        // }
        member.AddPredictionToDo();
        // Console.WriteLine("Anzahl Spiele in PredictionsToDo: " + member.GetPredictionsToDo().Count);
        // Console.WriteLine("Anzahl Spiele in PredictionsDone: "+ member.GetPredictionsDone().Count);
        List<FootballMatch?> predicted_matches_on_day = new List<FootballMatch?>();
        foreach (var predicition in member.GetPredictionsDone())
        {
            if (predicition.PredictedMatch.MatchDate.Date == DateTime.Now.Date)
            {
                predicted_matches_on_day.Add(predicition.PredictedMatch as FootballMatch);
            }
        }
        foreach (var predicition in member.GetArchivedPredictions())
        {
            if (predicition.PredictedMatch.MatchDate.Date == DateTime.Now.Date)
            {
                predicted_matches_on_day.Add(predicition.PredictedMatch as FootballMatch);
            }
        }

        List<FootballPrediction> football_predictions_on_day = new List<FootballPrediction>();
        foreach (var match in predicted_matches_on_day)
        {
            FootballPrediction? prediction = (FootballPrediction?)
                member.GetPredictionsDone().Find(p => p.PredictedMatch.MatchID == match.MatchID);
            if (prediction != null)
            {
                football_predictions_on_day.Add(prediction);
            }
        }
        foreach (var match in predicted_matches_on_day)
        {
            FootballPrediction? prediction = (FootballPrediction?)
                member
                    .GetArchivedPredictions()
                    .Find(p => p.PredictedMatch.MatchID == match.MatchID);
            if (prediction != null)
            {
                football_predictions_on_day.Add(prediction);
            }
        }

        string? member_forename = member.GetForename();
        int predictions_to_do_count = member.GetPredictionsToDo().Count;

        if (predictions_to_do_count == 0)
        {
            Console.WriteLine(
                $"Hello {member_forename}, there are no matches left to predict today!"
            );
            if (predicted_matches_on_day.Count > 0)
            {
                Console.WriteLine("Here is a list of the Predictions you already did:\n");
                foreach (var match in predicted_matches_on_day)
                {
                    FootballPrediction? prediction = football_predictions_on_day.Find(m =>
                        m.PredictedMatch.MatchID == match.MatchID
                    );

                    if (match != null)
                    {
                        Console.WriteLine(
                            $"{match.MatchDate:dd.MM.yyyy HH:mm} {match.HomeTeam} - {match.AwayTeam} | {prediction.PredictionHome}:{prediction.PredictionAway}"
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            $"{match.MatchDate:dd.MM.yyyy HH:mm} {match.HomeTeam} - {match.AwayTeam} | No prediction found"
                        );
                    }
                }
            }
        }
        else
        {
            Console.WriteLine(
                $"Hello {member_forename}, here are your matches you can predict today:\n"
            );

            var predictable_matches = new List<FootballMatch>();
            foreach (var match_to_predict in member.GetPredictionsToDo())
            {
                predictable_matches.Add(match_to_predict as FootballMatch);
            }

            for (
                int prediction_number = 0;
                prediction_number < predictions_to_do_count;
                prediction_number++
            )
            {
                string match_date = predictable_matches[prediction_number]
                    .MatchDate.ToString("HH:mm");
                Console.WriteLine(
                    $"{prediction_number + 1}: {predictable_matches[prediction_number].HomeTeam} - {predictable_matches[prediction_number].AwayTeam}, {match_date}"
                );
            }

            int match_number;
            while (true)
            {
                Console.Write(
                    "\nEnter the number of the match you want to predict or press [esc] to return to the main menu: "
                );
                var keyInfo = Console.ReadKey(true); // Read a key without displaying it

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return; // Exit the method to return to the main menu
                }

                if (char.IsDigit(keyInfo.KeyChar))
                {
                    Console.Write(keyInfo.KeyChar);
                    string input = keyInfo.KeyChar.ToString();

                    // Read additional digits if any
                    while (true)
                    {
                        keyInfo = Console.ReadKey(true);
                        if (char.IsDigit(keyInfo.KeyChar))
                        {
                            Console.Write(keyInfo.KeyChar);
                            input += keyInfo.KeyChar;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (
                        int.TryParse(input, out match_number)
                        && match_number > 0
                        && match_number <= predictable_matches.Count
                    )
                    {
                        break; // Valid match number entered, exit loop
                    }
                    else
                    {
                        Console.WriteLine(
                            "\nInvalid input. Please enter a valid match number or press [esc] to return to the main menu."
                        );
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please enter a valid match number.");
                }
            }

            FootballMatch? match =
                em_2024.Matches.Find(m =>
                    m.MatchID == predictable_matches[match_number - 1].MatchID
                ) as FootballMatch;
            if (match == null)
            {
                Console.WriteLine(
                    "Match not found in schedule.\nPress any button to return to the menu..."
                );
                Console.ReadKey();
                return;
            }
            Console.WriteLine();
            byte PredictionHome = GetPrediction($"{match.HomeTeam}");
            byte PredictionAway = GetPrediction($"{match.AwayTeam}");

            // Console.WriteLine(match.MatchID);
            // foreach (var match_to_find in member.GetPredictionsToDo())
            // {
            //     if (match_to_find.MatchID == match.MatchID)
            //     {
            //         Console.WriteLine("Match gefunden!");
            //         Console.WriteLine($"{match.MatchID} + {match}");
            //     }
            //     else
            //     {
            //         Console.WriteLine("Match nicht in der PredictionsToDo gefunden");
            //         Console.WriteLine($"{match.MatchID} + {match}");
            //     }
            // }

            // Console.WriteLine($"Übergebene MatchID: {match.MatchID}");
            member.ConvertPredictionsDone(match, PredictionHome, PredictionAway);
            Console.WriteLine("Prediction added successfully!");
        }

        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey();
    }

    private static byte GetPrediction(string team)
    {
        byte prediction;
        while (true)
        {
            Console.Write($"Enter Prediction for {team}: ");
            if (byte.TryParse(Console.ReadLine(), out prediction))
            {
                if (prediction > 9)
                {
                    Console.WriteLine(
                        $"You entered a high score for the {team} ({prediction}).\nAre you sure you want to continue?"
                    );
                    Console.WriteLine(
                        "Press any key to continue or [esc] to set a new prediction..."
                    );
                    var key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Escape)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            else
            {
                Console.WriteLine(
                    "Invalid input. Please enter a valid score (number from 0 to 255)."
                );
            }
        }
        return prediction;
    }

    private static void SaveAndExit(
        PredictionGame prediction_game,
        string memberFilePath,
        string predictionFilePath,
        string scoreFilePath
    )
    {
        Console.Clear();
        Console.WriteLine("Are you sure you want to save and exit?");
        Console.WriteLine("Press any key to confirm, 'Esc' to cancel.");

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        if (keyInfo.Key == ConsoleKey.Escape)
        {
            // Cancel the exit process and return to the main menu
            Console.WriteLine("Exit cancelled. Press any key to return to the main menu...");
            Console.ReadKey(); // Wait for user to acknowledge
            return; // Return to the main menu
        }
        else
        {
            // Save data to files
            CSVWriter<Match, Prediction>.WriteMemberData(memberFilePath, prediction_game);
            CSVWriter<Match, Prediction>.TrackFootballPredictionData(
                predictionFilePath,
                prediction_game
            );
            CSVWriter<Match, Prediction>.TrackScoreData(scoreFilePath, prediction_game);

            Console.WriteLine("Data saved successfully. Exiting...");
            Thread.Sleep(1000);
            Environment.Exit(0); // Ensure the application exits
        }
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
