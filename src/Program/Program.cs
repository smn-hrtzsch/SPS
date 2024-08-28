using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

public class Program
{
    static uint? member_id = null;

    public static void Main(string[] args)
    {
        // Set up email service and initialize prediction game
        EmailService emailService = new EmailService();
        PredictionGame prediction_game = new PredictionGame(emailService);
        bool login = false;
        DateTime yesterday = DateTime.Now.AddDays(-1);

        // Set up match factory for the CSVReader
        IMatchFactory<FootballMatch?> football_match_factory = new FootballMatchFactory();
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(football_match_factory);

        // File paths
        string PathToEM_2024File = "../../csv-files/EM_2024.csv";
        string PathToLaLiga_24_25File = "../../csv-files/LaLiga_24_25.csv";
        string PathToMemberDataFile = "../../csv-files/MemberData.csv";
        string PathToPredictionDataFile = "../../csv-files/PredictionData.csv";
        string PathToScoreDataFile = "../../csv-files/ScoreData.csv";

        // Load schedule, members, predictions, and scores
        Schedule<Match?> em_2024;
        Schedule<Match?> laliga_24_25;
        if (File.Exists(PathToEM_2024File) && File.Exists(PathToLaLiga_24_25File))
        {
            em_2024 = new Schedule<Match?>(
                PathToEM_2024File,
                SportsTypes.Football,
                ScheduleTypes.EM_2024
            );
            laliga_24_25 = new Schedule<Match?>(
                PathToLaLiga_24_25File,
                SportsTypes.Football,
                ScheduleTypes.La_Liga_24_25
            );
            // set schedule type for every match in every Schedule
            if (em_2024.Matches != null && laliga_24_25.Matches != null)
            {
                foreach (var match in em_2024.Matches)
                {
                    if (match != null)
                    {
                        match.ScheduleType = ScheduleTypes.EM_2024;
                    }
                }
                foreach (var match in laliga_24_25.Matches)
                {
                    if (match != null)
                    {
                        match.ScheduleType = ScheduleTypes.La_Liga_24_25;
                    }
                }
            }
            if (File.Exists(PathToMemberDataFile))
            {
                prediction_game.Members = CSVReader<Match?, Prediction?>.GetMemberDataFromCsvFile(
                    PathToMemberDataFile,
                    new List<Schedule<Match?>?> { em_2024, laliga_24_25 }
                );
            }
            if (File.Exists(PathToPredictionDataFile))
            {
                CSVReader<Match, Prediction>.GetFootballPredictionsFromCsvFile(
                    PathToPredictionDataFile,
                    prediction_game,
                    new List<Schedule<Match?>?> { em_2024, laliga_24_25 }
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
            string? input = Console.ReadLine();

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
                        Console.Write("\nEnter your [Email] or press [esc] to return: ");
                        string email = string.Empty;

                        while (true)
                        {
                            var predictionInput = Console.ReadKey(true);

                            if (predictionInput.Key == ConsoleKey.Escape)
                            {
                                email = string.Empty;
                                break; // Return to the start screen
                            }

                            if (predictionInput.Key == ConsoleKey.Enter)
                            {
                                Console.WriteLine();
                                break; // End input
                            }

                            if (predictionInput.Key == ConsoleKey.Backspace && email.Length > 0)
                            {
                                email = email.Substring(0, email.Length - 1);
                                Console.Write("\b \b"); // Erase the last character in the console
                            }
                            else if (!char.IsControl(predictionInput.KeyChar))
                            {
                                email += predictionInput.KeyChar;
                                Console.Write(predictionInput.KeyChar);
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
                                    "Now enter your [password] or press [esc] to return: "
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
                Console.Write("\nEnter your email address or press [esc] to return: ");
                var newEmailResult = ReadInputWithEscape();
                string newEmail = newEmailResult.input;
                if (string.IsNullOrEmpty(newEmail))
                {
                    continue; // Return to the start screen
                }

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
                    Console.Write("\nEnter your forename or press [esc] to return: ");
                    var forenameResult = ReadInputWithEscape(true); // Allow empty input for forename
                    if (forenameResult.isEscape)
                    {
                        continue; // If escape is pressed, return to the start screen
                    }
                    string newForename = forenameResult.input;

                    Console.Write("\nEnter your surname or press [esc] to return: ");
                    var surnameResult = ReadInputWithEscape(true); // Allow empty input for surname
                    if (surnameResult.isEscape)
                    {
                        continue; // If escape is pressed, return to the start screen
                    }
                    string newSurname = surnameResult.input;

                    while (true)
                    {
                        string newPassword = string.Empty;
                        Console.Write("\nEnter a password or press [esc] to return: ");
                        newPassword = ReadPasswordWithEscape();
                        if (string.IsNullOrEmpty(newPassword))
                        {
                            continue; // Return to the start screen
                        }

                        string verifiedPassword = string.Empty;
                        Console.Write("Verify your password or press [esc] to return: ");
                        verifiedPassword = ReadPasswordWithEscape();
                        if (string.IsNullOrEmpty(verifiedPassword))
                        {
                            continue; // Return to the start screen
                        }

                        if (newPassword == verifiedPassword)
                        {
                            prediction_game.Members.Add(
                                new Member<Match?, Prediction?>(
                                    newForename,
                                    newSurname,
                                    newEmail,
                                    newPassword
                                )
                            );
                            Console.WriteLine("\nRegistration successful!");
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
                Console.WriteLine("\nThat did not work. Please try again.\n");
                Thread.Sleep(1000);
            }
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
            Console.WriteLine("[4] Manage Schedules");
            Console.WriteLine("[5] Save and Exit");
            Console.Write("Select an option (1-5): ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayMembers(prediction_game);
                    break;
                case "2":
                    DisplayScores(prediction_game);
                    break;
                case "3":
                    AddPrediction(prediction_game);
                    break;
                case "4":
                    ManageSchedules(
                        prediction_game,
                        new List<Schedule<Match?>> { em_2024, laliga_24_25 }
                    );
                    break;
                case "5":
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
            if (prediction?.PredictionDate.Day == date.Day)
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
        foreach (var member in prediction_game.Members)
        {
            member.CalculateScores();
        }
        Console.Clear();
        Console.WriteLine("Scores:\n");

        // Determine the maximum width for the MemberID (Forename) column
        int maxMemberInfoLength =
            prediction_game
                .Members.Select(m => $"{m.MemberID} ({m.GetForename()})")
                .Max(s => s.Length) + 2; // +2 for padding

        // Calculate column widths based on the longest text in each column (header or data)
        List<int> columnWidths = new List<int>();

        foreach (var scoreType in prediction_game.ScheduleTypesList)
        {
            int maxScoreWidth = prediction_game
                .Members.Select(m =>
                    m.GetScores()
                        .FirstOrDefault(s => s.ScoreID == scoreType)
                        ?.AmountOfPoints.ToString()
                        .Length ?? 1
                )
                .Max();

            int columnWidth = Math.Max(
                10,
                Math.Max(scoreType.ToString().Length + 2, maxScoreWidth)
            );
            columnWidths.Add(columnWidth);
        }

        // Add column width for GesamtScore
        int totalColumnWidth = Math.Max(10, "GesamtScore".Length + 2);
        columnWidths.Add(totalColumnWidth);

        // Build the header row
        string header = $"| {"MemberID (Forename)".PadRight(maxMemberInfoLength)} ";
        int first_column_length = header.Length;
        for (int i = 0; i < prediction_game.ScheduleTypesList.Count; i++)
        {
            string scoreType = prediction_game.ScheduleTypesList[i].ToString();
            header +=
                $"| {scoreType.PadLeft((columnWidths[i] + scoreType.Length) / 2).PadRight(columnWidths[i])} ";
        }
        header +=
            $"| {"GesamtScore".PadLeft((totalColumnWidth + "GesamtScore".Length) / 2).PadRight(totalColumnWidth)} |";

        // Output the header and separator line
        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        // Output each member's score row
        foreach (var member in prediction_game.Members)
        {
            // Add padding to the MemberID (Forename) field
            string row = string.Empty;
            string memberInfo = $"{member.MemberID} ({member.GetForename()})";
            if (maxMemberInfoLength < first_column_length)
            {
                row = $"| {memberInfo.PadRight(first_column_length - 3)} ";
            }
            else
            {
                row = $"| {memberInfo.PadRight(maxMemberInfoLength)} ";
            }
            int totalScore = 0;

            for (int i = 0; i < prediction_game.ScheduleTypesList.Count; i++)
            {
                var scheduleType = prediction_game.ScheduleTypesList[i];
                var score = member.GetScores().FirstOrDefault(s => s.ScoreID == scheduleType);
                int points = score != null ? (int)score.AmountOfPoints : 0;
                totalScore += points;
                string pointsStr = points.ToString();
                row +=
                    $"| {pointsStr.PadLeft((columnWidths[i] + pointsStr.Length) / 2).PadRight(columnWidths[i])} ";
            }

            string totalScoreStr = totalScore.ToString();
            row +=
                $"| {totalScoreStr.PadLeft((totalColumnWidth + totalScoreStr.Length) / 2).PadRight(totalColumnWidth)} |";
            Console.WriteLine(row);
        }

        // Output the final separator line
        Console.WriteLine(new string('-', header.Length));
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }

    private static void AddPrediction(PredictionGame prediction_game)
    {
        var member = prediction_game.Members.Find(m => m.MemberID == member_id);
        if (member == null)
        {
            Console.WriteLine("Member not found.");
            Console.ReadKey();
            return;
        }

        member.AddPredictionToDo();

        while (true) // Hauptschleife für die Vorhersagen
        {
            // Listen für Spiele, die bereits vorhergesagt wurden, aufgeteilt nach Wettbewerb
            List<FootballMatch?> em2024_matches = new List<FootballMatch?>();
            List<FootballMatch?> laliga_matches = new List<FootballMatch?>();

            // Sammle die Spiele, die heute vorhergesagt wurden
            foreach (var prediction in member.GetPredictionsDone())
            {
                if (prediction?.PredictedMatch?.MatchDate.Date == DateTime.Now.Date)
                {
                    if (IsEM2024Match(prediction.PredictedMatch))
                    {
                        em2024_matches.Add(prediction.PredictedMatch as FootballMatch);
                    }
                    else
                    {
                        laliga_matches.Add(prediction.PredictedMatch as FootballMatch);
                    }
                }
            }

            foreach (var prediction in member.GetArchivedPredictions())
            {
                if (prediction?.PredictedMatch?.MatchDate.Date == DateTime.Now.Date)
                {
                    if (IsEM2024Match(prediction.PredictedMatch))
                    {
                        em2024_matches.Add(prediction.PredictedMatch as FootballMatch);
                    }
                    else
                    {
                        laliga_matches.Add(prediction.PredictedMatch as FootballMatch);
                    }
                }
            }

            List<FootballPrediction> football_predictions_on_day = new List<FootballPrediction>();

            // Fülle die Liste der Vorhersagen des Tages
            FillPredictionsForMatches(em2024_matches, football_predictions_on_day, member);
            FillPredictionsForMatches(laliga_matches, football_predictions_on_day, member);

            int predictions_to_do_count = member.GetPredictionsToDo().Count;
            int football_predictions_on_day_count = football_predictions_on_day.Count;

            Console.Clear();
            if (predictions_to_do_count == 0 && football_predictions_on_day_count == 0)
            {
                Console.WriteLine(
                    $"Hello {member.GetForename()}, there are no matches left to predict today!"
                );
                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
                return;
            }
            else
            {
                if (predictions_to_do_count > 0)
                {
                    Console.WriteLine(
                        $"Hello {member.GetForename()}, here are the matches you can predict today:\n"
                    );
                }

                var predictable_matches = new List<FootballMatch>();
                foreach (var match_to_predict in member.GetPredictionsToDo())
                {
                    if (match_to_predict is FootballMatch footballMatch)
                    {
                        predictable_matches.Add(footballMatch);
                    }
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

                // Ausgabe der bereits vorhergesagten Spiele nach Wettbewerb
                int next_prediction_number = predictions_to_do_count;
                next_prediction_number = DisplayPredictions(
                    "Here are your already predicted Matches for the schedule EM 2024:",
                    em2024_matches,
                    football_predictions_on_day,
                    next_prediction_number
                );
                next_prediction_number = DisplayPredictions(
                    "Here are your already predicted Matches for the schedule LaLiga:",
                    laliga_matches,
                    football_predictions_on_day,
                    next_prediction_number
                );

                int match_number = GetMatchNumberFromUser(
                    predictions_to_do_count,
                    football_predictions_on_day.Count
                );

                if (match_number == -1)
                    return; // Zurück zur Hauptschleife, wenn die Vorhersage abgebrochen wurde

                if (match_number <= predictions_to_do_count)
                {
                    FootballMatch? match = predictable_matches[match_number - 1];

                    if (match == null)
                    {
                        Console.WriteLine(
                            "\nMatch not found in schedule.\nPress any button to continue..."
                        );
                        Console.ReadKey();
                        continue; // Zurück zur Schleife, um erneut eine Vorhersage auszuwählen
                    }
                    Console.WriteLine();

                    if (match.MatchDate < DateTime.Now)
                    {
                        Console.WriteLine(
                            "\n\nYou cannot predict this match anymore, because it has already started or taken place."
                        );
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                        continue; // Zurück zur Schleife, um erneut eine Vorhersage auszuwählen
                    }

                    Console.WriteLine();

                    byte PredictionHome = GetPrediction($"{match.HomeTeam}");
                    if (PredictionHome == 255)
                    {
                        continue; // Abbruch der Vorhersage und Zurückkehren zur Schleife
                    }
                    byte PredictionAway = GetPrediction($"{match.AwayTeam}");
                    if (PredictionAway == 255)
                    {
                        continue; // Abbruch der Vorhersage und Zurückkehren zur Schleife
                    }

                    member.ConvertPredictionsDone(match, PredictionHome, PredictionAway);
                    Console.WriteLine("\nPrediction added successfully!");
                    Thread.Sleep(1000);
                }
                else
                {
                    int index = match_number - predictions_to_do_count - 1;

                    FootballPrediction? prediction = football_predictions_on_day[index];
                    FootballMatch? match = prediction?.PredictedMatch as FootballMatch;

                    if (prediction != null && match != null)
                    {
                        if (prediction?.PredictedMatch?.MatchDate < DateTime.Now)
                        {
                            Console.WriteLine(
                                "\n\nYour Prediction cannot be changed, because the match has already started or taken place."
                            );
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                            continue; // Zurück zur Schleife, um erneut eine Vorhersage auszuwählen
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine();
                            byte NewPredictionHome = GetPrediction(match.HomeTeam);
                            if (NewPredictionHome == 255)
                            {
                                continue; // Abbruch der Vorhersage und Zurückkehren zur Schleife
                            }
                            byte NewPredictionAway = GetPrediction(match.AwayTeam);
                            if (NewPredictionAway == 255)
                            {
                                continue; // Abbruch der Vorhersage und Zurückkehren zur Schleife
                            }

                            FootballPrediction.ChangePrediction(
                                NewPredictionHome,
                                NewPredictionAway,
                                prediction
                            );
                            Console.WriteLine("\nPrediction was successfully changed");
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }
    }

    // Hilfsmethode zur Überprüfung, ob es sich um ein EM 2024 Spiel handelt
    private static bool IsEM2024Match(Match match)
    {
        return match.ScheduleType == ScheduleTypes.EM_2024; // Anpassung je nach Feldname oder Logik
    }

    // Hilfsmethode zur Ausgabe der Vorhersagen
    private static int DisplayPredictions(
        string header,
        List<FootballMatch?> matches,
        List<FootballPrediction> football_predictions_on_day,
        int prediction_start_number
    )
    {
        if (matches.Count > 0)
        {
            Console.WriteLine($"\n{header}\n");
            for (int i = 0; i < matches.Count; i++)
            {
                FootballMatch? match = matches[i];
                FootballPrediction? prediction = football_predictions_on_day.FirstOrDefault(p =>
                    p?.PredictedMatch?.MatchID == match?.MatchID
                );

                if (prediction != null && match != null)
                {
                    Console.WriteLine(
                        $"{prediction_start_number + i + 1}: {match.MatchDate:dd.MM.yyyy HH:mm} {match.HomeTeam} - {match.AwayTeam} | {prediction.PredictionHome}:{prediction.PredictionAway}"
                    );
                }
                else if (match != null)
                {
                    Console.WriteLine(
                        $"{prediction_start_number + i + 1}: {match.MatchDate:dd.MM.yyyy HH:mm} {match.HomeTeam} - {match.AwayTeam} | No prediction found"
                    );
                }
            }
            return prediction_start_number + matches.Count;
        }
        return prediction_start_number;
    }

    // Hilfsmethode zum Füllen der Vorhersagen für die Spiele des Tages
    private static void FillPredictionsForMatches(
        List<FootballMatch?> matches,
        List<FootballPrediction> football_predictions_on_day,
        Member<Match?, Prediction?> member
    )
    {
        foreach (var match in matches)
        {
            FootballPrediction? prediction =
                member
                    ?.GetPredictionsDone()
                    ?.FirstOrDefault(p => p?.PredictedMatch?.MatchID == match?.MatchID)
                as FootballPrediction;
            if (prediction != null)
            {
                football_predictions_on_day.Add(prediction);
            }
        }
        foreach (var match in matches)
        {
            FootballPrediction? prediction =
                member
                    ?.GetArchivedPredictions()
                    .FirstOrDefault(p => p?.PredictedMatch?.MatchID == match?.MatchID)
                as FootballPrediction;
            if (prediction != null)
            {
                football_predictions_on_day.Add(prediction);
            }
        }
    }

    // Methode zur Abfrage der Spielnummer vom Benutzer
    private static int GetMatchNumberFromUser(
        int predictions_to_do_count,
        int predicted_matches_on_day_count
    )
    {
        int match_number;
        while (true)
        {
            Console.WriteLine(
                "\nEnter the number of the match you want to predict or change your already predicted matches."
            );
            Console.Write("If you want to cancel the prediction, press [esc]: ");
            var keyInfo = Console.ReadKey(true); // Read a key without displaying it

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                match_number = -1;
                break; // Abbruch der aktuellen Vorhersage, aber in der Schleife bleiben
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
                    && match_number <= predicted_matches_on_day_count + predictions_to_do_count
                )
                {
                    break; // Valid match number entered, exit loop
                }
                else
                {
                    Console.WriteLine(
                        "\nInvalid input. Please enter a valid match number or press [esc] to cancel."
                    );
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a valid match number.");
            }
        }
        return match_number;
    }

    // Methode zur Abfrage der Vorhersage für ein Team
    private static byte GetPrediction(string? team)
    {
        byte prediction;
        while (true)
        {
            string predictionstr = string.Empty;
            Console.Write($"Enter Prediction for {team} or press [esc] to cancel: ");

            while (true)
            {
                var predictionInput = Console.ReadKey(true);

                if (predictionInput.Key == ConsoleKey.Escape)
                {
                    return 255; // Use 255 as a special code indicating the user wants to cancel
                }

                if (predictionInput.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break; // End input
                }

                if (predictionInput.Key == ConsoleKey.Backspace && predictionstr.Length > 0)
                {
                    predictionstr = predictionstr.Substring(0, predictionstr.Length - 1);
                    Console.Write("\b \b"); // Erase the last character in the console
                }
                else if (!char.IsControl(predictionInput.KeyChar))
                {
                    predictionstr += predictionInput.KeyChar;
                    Console.Write(predictionInput.KeyChar);
                }
            }

            if (byte.TryParse(predictionstr, out prediction))
            {
                if (prediction > 9)
                {
                    Console.WriteLine(
                        $"You entered a high score for {team} ({prediction}).\nAre you sure you want to continue?"
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
                    "Invalid input. Please enter a valid score (number from 0 to 254)."
                );
            }
        }
        return prediction;
    }

    private static void ManageSchedules(
        PredictionGame prediction_game,
        List<Schedule<Match?>> schedules
    )
    {
        Console.Clear();

        var member = prediction_game.Members.Find(m => m.MemberID == member_id);

        List<ScheduleTypes> MemberScheduleTypes = new List<ScheduleTypes>();
        List<Schedule<Match?>?>? ParticipatingSchedules = member?.GetParticipatingSchedules();

        if (ParticipatingSchedules != null)
        {
            foreach (var schedule in ParticipatingSchedules)
            {
                if (schedule != null)
                {
                    MemberScheduleTypes.Add(schedule.ScheduleID);
                }
            }
        }

        Console.WriteLine(
            $"Hey, {member?.GetForename()}. Here you can manage the schedules you want to predict..."
        );
        Console.WriteLine("All schedules included in SPS: \n");

        if (prediction_game.ScheduleTypesList != null)
        {
            for (int i = 0; i < prediction_game.ScheduleTypesList.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {prediction_game.ScheduleTypesList[i]}");
            }
        }

        if (member?.GetScores().Count == 0)
        {
            Console.WriteLine("\nYou are currently not participating in any schedule.");
        }
        else
        {
            Console.WriteLine(
                "\nAnd these are the schedules you are currently participating in: \n"
            );

            for (int i = 0; i < MemberScheduleTypes.Count; i++)
            {
                Console.WriteLine($"\t{MemberScheduleTypes[i]}");
            }
        }

        if (prediction_game.ScheduleTypesList != null)
        {
            int schedule_number = GetScheduleNumberFromUser(
                prediction_game.ScheduleTypesList.Count
            );
            if (schedule_number == -1)
            {
                return;
            }
            if (schedule_number <= prediction_game.ScheduleTypesList.Count)
            {
                while (true)
                {
                    Console.Write("\nDo you want to add [1] or remove [2] this schedule? ");
                    var input = Console.ReadLine();
                    if (input == "1")
                    {
                        if (
                            !MemberScheduleTypes.Contains(
                                prediction_game.ScheduleTypesList[schedule_number - 1]
                            )
                        )
                        {
                            member?.AddParticipatingSchedule(
                                schedules[schedule_number - 1],
                                prediction_game.ScheduleTypesList[schedule_number - 1]
                            );
                            Console.WriteLine("\nSchedule was added successfully.");
                            Console.WriteLine("Press any key to return to the main menu...");
                            Console.ReadKey();
                            return;
                        }
                        else
                        {
                            Console.WriteLine(
                                "\nYou already participate in predicting this schedule."
                            );
                            Console.WriteLine("Press any key to return to the main menu...");
                            Console.ReadKey();
                            return;
                        }
                    }
                    else if (input == "2")
                    {
                        if (
                            MemberScheduleTypes.Contains(
                                prediction_game.ScheduleTypesList[schedule_number - 1]
                            )
                        )
                        {
                            member?.RemoveParticipatingSchedule(
                                prediction_game.ScheduleTypesList[schedule_number - 1]
                            );
                            Console.WriteLine("\nSchedule was removed successfully.");
                            Console.WriteLine("Press any key to return to the main menu...");
                            Console.ReadKey();
                            return;
                        }
                        else
                        {
                            Console.WriteLine(
                                "\nYou are not participating in this schedule, so it cannot not be removed."
                            );
                            Console.WriteLine("Press any key to return to the main menu...");
                            Console.ReadKey();
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine(
                            "\nInvalid input, please enter 1 for adding the schedule or 2 for removing."
                        );
                        Console.WriteLine("Press any key to return to choose again.");
                        Console.ReadKey();
                        continue;
                    }
                }
            }
        }
    }

    // Methode zur Abfrage der Spielnummer vom Benutzer
    private static int GetScheduleNumberFromUser(int possible_schedules_count)
    {
        int schedule_number;
        while (true)
        {
            Console.WriteLine("\nEnter the number of the schedule you want to add or remove.");
            Console.Write("If you want to cancel, press [esc]: ");
            var keyInfo = Console.ReadKey(true); // Read a key without displaying it

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                schedule_number = -1;
                break; // Abbruch der aktuellen Vorhersage, aber in der Schleife bleiben
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
                    int.TryParse(input, out schedule_number)
                    && schedule_number > 0
                    && schedule_number <= possible_schedules_count
                )
                {
                    break; // Valid match number entered, exit loop
                }
                else
                {
                    Console.WriteLine(
                        "\nInvalid input. Please enter a valid schedule number or press [esc] to cancel."
                    );
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a valid schedule number.");
            }
        }
        return schedule_number;
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

    private static (string input, bool isEscape) ReadInputWithEscape(bool allowEmpty = false)
    {
        string input = string.Empty;
        while (true)
        {
            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return (string.Empty, true); // Escape pressed, return empty string and set flag
            }

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                if (allowEmpty || !string.IsNullOrEmpty(input))
                {
                    return (input, false); // Return input with no escape flag
                }
            }

            if (keyInfo.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input.Substring(0, input.Length - 1);
                Console.Write("\b \b"); // Erase the last character in the console
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                input += keyInfo.KeyChar;
                Console.Write(keyInfo.KeyChar);
            }
        }
    }

    private static string ReadPasswordWithEscape()
    {
        string password = string.Empty;
        while (true)
        {
            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return string.Empty; // Escape pressed, return an empty string
            }

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break; // End input
            }

            if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b"); // Erase the last character in the console
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                password += keyInfo.KeyChar;
                Console.Write("*"); // Mask the password characters
            }
        }
        return password;
    }
}
