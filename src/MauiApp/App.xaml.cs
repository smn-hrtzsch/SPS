using Microsoft.Maui.Controls;

namespace MauiApplication;

public partial class App : Application
{
    public static DataService? DataService { get; private set; }
    public static PredictionGame? PredictionGame { get; private set; }

    public App()
    {
        InitializeComponent();

        // Initialize PredictionGame
        PredictionGame = new PredictionGame(new EmailService());

        // Initialize DataService with paths to CSV files
        string baseDirectory = FileSystem.AppDataDirectory; // Use a platform-independent path
        Console.WriteLine($"Base Directory: {baseDirectory}");
        string pathToMemberDataFile = Path.Combine(baseDirectory, "MemberData.csv");
        string pathToPredictionDataFile = Path.Combine(baseDirectory, "PredictionData.csv");
        string pathToScoreDataFile = Path.Combine(baseDirectory, "ScoreData.csv");
        string pathToEM_2024File = Path.Combine(baseDirectory, "EM_2024.csv");
        string pathToLaLiga_24_25_File = Path.Combine(baseDirectory, "LaLiga_24_25.csv");

        DataService = new DataService(
            pathToMemberDataFile,
            pathToPredictionDataFile,
            pathToScoreDataFile,
            pathToEM_2024File,
            pathToLaLiga_24_25_File
        );

        // Load initial data
        PredictionGame.Members = DataService.LoadMembers();
        DataService.LoadPredictions(PredictionGame);
        DataService.LoadScores(PredictionGame);

        MainPage = new NavigationPage(new LoginPage()); // start with LoginPage
    }
}
