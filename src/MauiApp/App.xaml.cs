using System.Diagnostics;
using System.Reflection;
using MauiApplication.Helpers;
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

        // Get the platform-independent app data directory
        string baseDirectory = FileSystem.AppDataDirectory;

        // Debug: Check all embedded resources
        var assembly = Assembly.GetExecutingAssembly();
        foreach (var resourceName in assembly.GetManifestResourceNames())
        {
            Debug.WriteLine($"Found embedded resource: {resourceName}");
        }

        // Extract CSV files from embedded resources
        try
        {
            ResourceHelper.ExtractEmbeddedResource(
                "MauiApplication.MemberData.csv",
                Path.Combine(baseDirectory, "MemberData.csv")
            );
            ResourceHelper.ExtractEmbeddedResource(
                "MauiApplication.PredictionData.csv",
                Path.Combine(baseDirectory, "PredictionData.csv")
            );
            ResourceHelper.ExtractEmbeddedResource(
                "MauiApplication.ScoreData.csv",
                Path.Combine(baseDirectory, "ScoreData.csv")
            );
            ResourceHelper.ExtractEmbeddedResource(
                "MauiApplication.EM_2024.csv",
                Path.Combine(baseDirectory, "EM_2024.csv")
            );
            ResourceHelper.ExtractEmbeddedResource(
                "MauiApplication.LaLiga_24_25.csv",
                Path.Combine(baseDirectory, "LaLiga_24_25.csv")
            );
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error extracting resources: {ex.Message}");
        }

        // Initialize DataService with paths to CSV files in the AppDataDirectory
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
        try
        {
            PredictionGame.Members = DataService.LoadMembers();
            DataService.LoadPredictions(PredictionGame);
            DataService.LoadScores(PredictionGame);
        }
        catch (InvalidDataException ex)
        {
            Debug.WriteLine($"Error loading data: {ex.Message}");
            // Handle exception (show a message to the user, log it, etc.)
        }

        MainPage = new NavigationPage(new LoginPage()); // start with LoginPage
    }
}
