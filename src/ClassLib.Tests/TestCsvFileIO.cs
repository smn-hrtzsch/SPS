using Microsoft.Win32.SafeHandles;

namespace ClassLib.Tests;

public class TestCsvFileIO
{
    private static IMatchFactory<FootballMatch> footballMatchFactory = new FootballMatchFactory();

    [Fact]
    public static void TestGetMatchDataFromCsvFile()
    {
        string[] MatchArray = CSVReader<FootballMatch, FootballPrediction>.GetMatchDataFromCsvFile(
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
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(footballMatchFactory);
        List<FootballMatch> schedule = CSVReader<
            FootballMatch,
            FootballPrediction
        >.GetScheduleFromCsvFile("../../../../../csv-files/EM_2024.csv", SportsTypes.Football);

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
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(footballMatchFactory);
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
        PredictionGame test_prediction_game = new PredictionGame(new EmailService());

        test_prediction_game.Register(
            new TestMember("Artim", "Meyer", "Artim.Meyer@student.tu-freiberg.de", "SPSistCool")
        );
        test_prediction_game.Register(
            new TestMember(
                "Simon",
                "Hörtzsch",
                "Simon.Hoertzsch@student.tu-freiberg.de",
                "SPSistCool1234"
            )
        );

        string testFilePath = "../../../MembersTest.csv";

        CSVWriter<Match, Prediction>.WriteMemberData(testFilePath, test_prediction_game);

        var lines = File.ReadAllLines(testFilePath);

        Assert.Equal(3, lines.Length);
        Assert.Equal("MemberID;Forename;Surname;Email Address;Password", lines[0]);
        Assert.Equal(
            $"{test_prediction_game.Members[0].MemberID};Artim;Meyer;Artim.Meyer@student.tu-freiberg.de;SPSistCool",
            lines[1]
        );
        Assert.Equal(
            $"{test_prediction_game.Members[1].MemberID};Simon;Hörtzsch;Simon.Hoertzsch@student.tu-freiberg.de;SPSistCool1234",
            lines[2]
        );
    }

    [Fact]
    public static void TestTrackScoreData()
    {
        PredictionGame test_prediction_game = new PredictionGame(new EmailService());

        test_prediction_game.Register(
            new TestMember("Artim", "Meyer", "Artim.Meyer@student.tu-freiberg.de", "SPSistCool")
        );
        test_prediction_game.Register(
            new TestMember(
                "Simon",
                "Hörtzsch",
                "Simon.Hoertzsch@student.tu-freiberg.de",
                "SPSistCool1234"
            )
        );

        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(footballMatchFactory);
        Schedule<Match> schedule = new Schedule<Match>(
            "../../../../../csv-files/EM_2024.csv",
            SportsTypes.Football,
            ScheduleTypes.EM_2024
        );

        test_prediction_game.Members[0].AddParticipatingSchedule(schedule, schedule.ScheduleID);
        test_prediction_game.Members[1].AddParticipatingSchedule(schedule, schedule.ScheduleID);

        string testFilePath = "../../../MemberScoresTest.csv";

        CSVWriter<FootballMatch, Prediction>.TrackScoreData(testFilePath, test_prediction_game);

        var lines = File.ReadAllLines(testFilePath);

        Assert.Equal(3, lines.Length);
        Assert.Equal("MemberID;EM_2024", lines[0]);
        Assert.Equal($"{test_prediction_game.Members[0].MemberID};0", lines[1]);
        Assert.Equal($"{test_prediction_game.Members[1].MemberID};0", lines[2]);
    }

    [Fact]
    public static void TestTrackFootballPredictionData()
    {
        PredictionGame test_prediction_game = new PredictionGame(new EmailService());

        test_prediction_game.Register(
            new TestMember("Artim", "Meyer", "Artim.Meyer@student.tu-freiberg.de", "SPSistCool")
        );
        test_prediction_game.Register(
            new TestMember(
                "Simon",
                "Hörtzsch",
                "Simon.Hoertzsch@student.tu-freiberg.de",
                "SPSistCool1234"
            )
        );

        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(footballMatchFactory);
        Schedule<Match> schedule = new Schedule<Match>(
            "../../../EM_2024Test.csv",
            SportsTypes.Football,
            ScheduleTypes.EM_2024
        );

        test_prediction_game.Members[0].AddParticipatingSchedule(schedule, ScheduleTypes.EM_2024);
        test_prediction_game.Members[1].AddParticipatingSchedule(schedule, ScheduleTypes.EM_2024);

        test_prediction_game.Members[0].AddPredictionToDo();
        test_prediction_game.Members[1].AddPredictionToDo();

        Assert.Equal(2, test_prediction_game.Members[0].GetPredictionsToDo().Count);
        Assert.Equal(2, test_prediction_game.Members[1].GetPredictionsToDo().Count);

        List<Match> predictionsToDoMember1 = test_prediction_game.Members[0].GetPredictionsToDo();
        List<Match> predictionsToDoMember2 = test_prediction_game.Members[1].GetPredictionsToDo();

        test_prediction_game
            .Members[0]
            .ConvertPredictionsDone(predictionsToDoMember1[0].MatchID, 1, 2);
        test_prediction_game
            .Members[0]
            .ConvertPredictionsDone(predictionsToDoMember1[1].MatchID, 2, 3);
        test_prediction_game
            .Members[1]
            .ConvertPredictionsDone(predictionsToDoMember2[0].MatchID, 3, 0);
        test_prediction_game
            .Members[1]
            .ConvertPredictionsDone(predictionsToDoMember2[1].MatchID, 4, 2);

        test_prediction_game.Members[0].CalculateScores();
        test_prediction_game.Members[1].CalculateScores();

        string testFilePath = "../../../MemberPredictionsTest.csv";

        CSVWriter<FootballMatch, Prediction>.TrackFootballPredictionData(
            testFilePath,
            test_prediction_game
        );

        var lines = File.ReadAllLines(testFilePath);

        // Überprüfen der Kopfzeile
        Assert.Equal(
            $"Predicted Match;{test_prediction_game.Members[0].MemberID};{test_prediction_game.Members[1].MemberID};CalculateScore() already DONE;MatchData;PredictionDate",
            lines[0]
        );

        FootballPrediction prediction1 = (FootballPrediction)
            test_prediction_game.Members[0].GetArchivedPredictions()[0];
        FootballPrediction prediction2 = (FootballPrediction)
            test_prediction_game.Members[0].GetArchivedPredictions()[1];
        FootballPrediction prediction3 = (FootballPrediction)
            test_prediction_game.Members[1].GetArchivedPredictions()[0];
        FootballPrediction prediction4 = (FootballPrediction)
            test_prediction_game.Members[1].GetArchivedPredictions()[1];

        // Beispielwerte überprüfen (Hier musst du sicherstellen, dass die Vorhersagen vorhanden sind)
        Assert.Equal(
            $"{prediction1.HomeTeam} - {prediction1.AwayTeam};{prediction1.PredictionHome}:{prediction1.PredictionAway};{prediction3.PredictionHome}:{prediction3.PredictionAway};1;{prediction1.PredictedMatch.ToString()};{prediction1.PredictionDate}",
            lines[1]
        ); // Beispielwerte, passe sie an deine Daten an
        Assert.Equal(
            $"{prediction2.HomeTeam} - {prediction2.AwayTeam};{prediction2.PredictionHome}:{prediction2.PredictionAway};{prediction4.PredictionHome}:{prediction4.PredictionAway};1;{prediction2.PredictedMatch.ToString()};{prediction2.PredictionDate}",
            lines[2]
        ); // Beispielwerte, passe sie an deine Daten an
    }
}
