namespace ClassLib.Tests;

public class UnitTest1
{
    private static IMatchFactory<FootballMatch> footballMatchFactory = new FootballMatchFactory();

    [Fact]
    public static void TestGetMatchDataFromCsvFile()
    {
        string[] MatchArray = CSVReader<FootballMatch>.GetMatchDataFromCsvFile(
            "../../../../../csv-files/EM_2024.csv",
            1
        );
        Assert.True(MatchArray[0] == "14/06/2024 21:00");
        Assert.True(MatchArray[1] == "Germany");
        Assert.True(MatchArray[2] == "Scotland");
        Assert.True(MatchArray[3] == "5");
        Assert.True(MatchArray[4] == "1");
    }

    [Fact]
    public static void TestFootballMatchCtor()
    {
        FootballMatch match1 = new FootballMatch(
            "../../../../../csv-files/EM_2024.csv",
            1,
            SportsTypes.Football
        );
        DateTime expectedDate = new DateTime(2024, 6, 14, 21, 0, 0);

        Assert.True(match1.MatchDate == expectedDate);
        Assert.True(match1.HomeTeam == "Germany");
        Assert.True(match1.AwayTeam == "Scotland");
        Assert.True(match1.ResultTeam1 == 5);
        Assert.True(match1.ResultTeam2 == 1);
    }

    [Fact]
    public static void TestGetScheduleFromCsvFile()
    {
        CSVReader<FootballMatch>.SetMatchFactory(footballMatchFactory);
        List<FootballMatch> schedule = CSVReader<FootballMatch>.GetScheduleFromCsvFile(
            "../../../../../csv-files/EM_2024.csv",
            SportsTypes.Football
        );

        Assert.True(schedule.Count == 51);
        foreach (var match in schedule)
        {
            Assert.True(
                match.MatchID == (uint)match.GetHashCode(),
                $"Expected ID: {match.MatchID}, Actual ID: {match.GetHashCode()}"
            );
        }
    }

    [Fact]
    public static void TestUpdateSchedule()
    {
        CSVReader<FootballMatch>.SetMatchFactory(footballMatchFactory);
        List<FootballMatch> schedule = CSVReader<FootballMatch>.GetScheduleFromCsvFile(
            "../../../../../csv-files/EM_2024.csv",
            SportsTypes.Football
        );
        string testFilePath = "../../../../../csv-files/EM_2024_updated.csv";

        CSVWriter<FootballMatch>.UpdateSchedule(testFilePath, schedule);

        Assert.True(File.Exists(testFilePath), "CSV file was not created.");
        var lines = File.ReadAllLines(testFilePath);
        Assert.Equal(52, lines.Length);
        Assert.Equal(
            "MatchID;Date;Home Team;Away Team;Result Home Team;Result Away Team;Result Home Team Penalties;Result Away Team Penalties",
            lines[0]
        );
        Assert.Equal(schedule[0].ToString(), lines[1]);
        Assert.Equal(schedule[1].ToString(), lines[2]);

        File.Delete(testFilePath);
    }

    [Fact]
    public void TestDeleteScheduleFile()
    {
        string testFilePath = "../../../../../csv-files/TestFile.csv";
        File.WriteAllText(testFilePath, "Temporary file content");

        CSVWriter<Match>.DeleteScheduleFile(testFilePath);

        Assert.False(File.Exists(testFilePath), "CSV file was not deleted.");
    }

    // [Fact]
    // public static void TestMail()
    // {
    //     EmailService HeutigeMail = new EmailService();
    //     HeutigeMail.SendEmail(
    //         "artimmeyer@gmail.com",
    //         "sportspredictionsystem@gmail.com",
    //         "test email",
    //         "Hallo Artim. Diese Mail wird per C# gesendet!"
    //     );
    // }
}
