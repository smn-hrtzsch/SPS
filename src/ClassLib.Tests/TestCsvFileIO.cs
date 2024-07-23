using Microsoft.Win32.SafeHandles;

namespace ClassLib.Tests;

public class TestCsvFileIO
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
        Schedule<FootballMatch> schedule = new Schedule<FootballMatch>(
            "../../../../../csv-files/EM_2024.csv",
            SportsTypes.Football,
            ScheduleTypes.EM_2024
        );
        string testFilePath = "../../../../../csv-files/EM_2024_updated.csv";

        CSVWriter<FootballMatch, Prediction>.UpdateSchedule(testFilePath, schedule.Matches);

        Assert.True(File.Exists(testFilePath), "CSV file was not created.");
        var lines = File.ReadAllLines(testFilePath);
        Assert.Equal(52, lines.Length);
        Assert.Equal(
            "MatchID;Date;Home Team;Away Team;Result Home Team;Result Away Team;Result Home Team Penalties;Result Away Team Penalties",
            lines[0]
        );
        Assert.Equal(schedule.Matches[0].ToString(), lines[1]);
        Assert.Equal(schedule.Matches[1].ToString(), lines[2]);

        File.Delete(testFilePath);
    }

    [Fact]
    public void TestDeleteScheduleFile()
    {
        string testFilePath = "../../../../../csv-files/TestFile.csv";
        File.WriteAllText(testFilePath, "Temporary file content");

        CSVWriter<FootballMatch, Prediction>.DeleteFile(testFilePath);

        Assert.False(File.Exists(testFilePath), "CSV file was not deleted.");
    }

    [Fact]
    public static void TestWriteMemberData()
    {
        List<Member<Prediction, Match>> testMembers = new List<Member<Prediction, Match>>()
        {
            new TestMember("Artim", "Meyer", "Artim.Meyer@student.tu-freiberg.de", "SPSistCool"),
            new TestMember(
                "Simon",
                "Hörtzsch",
                "Simon.Hoertzsch@student.tu-freiberg.de",
                "SPSistCool1234"
            )
        };

        string testFilePath = "../../../MembersTest.csv";

        CSVWriter<Match, Prediction>.WriteMemberData(testFilePath, testMembers);

        var lines = File.ReadAllLines(testFilePath);

        Assert.Equal(3, lines.Length);
        Assert.Equal("MemberID;Forename;Surname;Email Address;Password", lines[0]);
        Assert.Equal(
            $"{testMembers[0].MemberID};Artim;Meyer;Artim.Meyer@student.tu-freiberg.de;SPSistCool",
            lines[1]
        );
        Assert.Equal(
            $"{testMembers[1].MemberID};Simon;Hörtzsch;Simon.Hoertzsch@student.tu-freiberg.de;SPSistCool1234",
            lines[2]
        );
    }

    [Fact]
    public static void TestTrackScoreData()
    {
        PredictionGame test_prediction_game = new PredictionGame(new EmailService());

        List<Member<Prediction, FootballMatch>> testMembers = new List<
            Member<Prediction, FootballMatch>
        >()
        {
            new Member<Prediction, FootballMatch>(
                "Artim",
                "Meyer",
                "Artim.Meyer@student.tu-freiberg.de",
                "SPSistCool"
            ),
            new Member<Prediction, FootballMatch>(
                "Simon",
                "Hörtzsch",
                "Simon.Hoertzsch@student.tu-freiberg.de",
                "SPSistCool1234"
            )
        };

        CSVReader<FootballMatch>.SetMatchFactory(footballMatchFactory);
        Schedule<FootballMatch> schedule = new Schedule<FootballMatch>(
            "../../../../../csv-files/EM_2024.csv",
            SportsTypes.Football,
            ScheduleTypes.EM_2024
        );

        testMembers[0].AddParticipatingSchedule(schedule, schedule.ScheduleID);
        testMembers[1].AddParticipatingSchedule(schedule, schedule.ScheduleID);

        string testFilePath = "../../../MemberScoresTest.csv";

        CSVWriter<FootballMatch, Prediction>.TrackScoreData(
            testFilePath,
            testMembers,
            test_prediction_game
        );

        var lines = File.ReadAllLines(testFilePath);

        Assert.Equal(3, lines.Length);
        Assert.Equal("MemberID;EM_2024", lines[0]);
        Assert.Equal($"{testMembers[0].MemberID};0", lines[1]);
        Assert.Equal($"{testMembers[1].MemberID};0", lines[2]);
    }
}
