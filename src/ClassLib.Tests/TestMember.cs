using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;

public class TestMember : Member<Match, Prediction>
{
    public TestMember(string forename, string surname, string emailaddress, string password)
        : base(forename, surname, emailaddress, password) { }

    public List<Schedule<Match>> ParticipatingSchedulesTest
    {
        get { return ParticipatingSchedules; }
    }

    public List<Match> PredictionsToDoTest
    {
        get { return PredictionsToDo; }
    }

    public List<Prediction> PredictionsDoneTest
    {
        get { return PredictionsDone; }
    }

    public List<Prediction> ArchivedPredictiontest
    {
        get { return ArchivedPredictions; }
    }

    public List<Score> ScoresTest
    {
        get { return Scores; }
    }
}

public class TestScore : Score
{
    public TestScore(ScheduleTypes predicted_schedule)
        : base(predicted_schedule) { }

    public uint AmountOfPointsTest
    {
        get { return AmountOfPoints; }
    }
}

public class MemberTest
{
    private static IMatchFactory<FootballMatch> footballMatchFactory = new FootballMatchFactory();

    [Fact]
    public void TestMemberCtor()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";
        string password = "1234";

        TestMember TestMember = new TestMember(vorname, nachname, email, password);
        Assert.True(TestMember.MemberID == (uint)TestMember.GetHashCode());
    }

    [Fact]
    public void TestMemmberAddParticipatingSchedule()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";
        string password = "1234";

        TestMember TestMember = new TestMember(vorname, nachname, email, password);
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(footballMatchFactory);
        Schedule<Match> schedule = new Schedule<Match>(
            "../../../EM_2024Test.csv",
            SportsTypes.Football,
            ScheduleTypes.EM_2024
        );

        TestMember.AddParticipatingSchedule(schedule, ScheduleTypes.EM_2024);
        Assert.True(TestMember.ParticipatingSchedulesTest.Count == 1);
        Assert.True(TestMember.ScoresTest.Count == 1);
    }

    [Fact]
    public void TestMemberRemoveParticipatingSchedule()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";
        string password = "1234";

        TestMember TestMember = new TestMember(vorname, nachname, email, password);
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(footballMatchFactory);
        Schedule<Match> schedule = new Schedule<Match>(
            "../../../EM_2024Test.csv",
            SportsTypes.Football,
            ScheduleTypes.EM_2024
        );

        TestMember.AddParticipatingSchedule(schedule, ScheduleTypes.EM_2024);
        TestMember.RemoveParticipatingSchedule(ScheduleTypes.EM_2024);
        Assert.True(TestMember.ParticipatingSchedulesTest.Count == 0);
    }

    [Fact]
    public void TestMemberAddPredictionToDo()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";
        string password = "1234";

        TestMember TestMember = new TestMember(vorname, nachname, email, password);
        CSVReader<FootballMatch, FootballPrediction>.SetMatchFactory(footballMatchFactory);
        Schedule<Match> schedule = new Schedule<Match>(
            "../../../EM_2024Test.csv",
            SportsTypes.Football,
            ScheduleTypes.EM_2024
        );

        TestMember.AddParticipatingSchedule(schedule, ScheduleTypes.EM_2024);
        TestMember.AddPredictionToDo();
        Assert.True(
            TestMember.PredictionsToDoTest.Count == schedule.GetMatchesOnDay().Count,
            "GetMatchesOnDay() or the Member Reference does not work"
        );
    }

    [Fact]
    public void TestMemberRemovePredictionToDo()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";
        string password = "1234";

        TestMember TestMember = new TestMember(vorname, nachname, email, password);

        FootballMatch match1 = new FootballMatch(
            "../../../EM_2024Test.csv",
            1,
            SportsTypes.Football
        );
        FootballMatch match2 = new FootballMatch(
            "../../../EM_2024Test.csv",
            51,
            SportsTypes.Football
        );

        TestMember.PredictionsToDoTest.Add(match1);
        TestMember.PredictionsToDoTest.Add(match2);

        Assert.True(TestMember.PredictionsToDoTest.Count == 2);
        TestMember.RemovePredictionToDo(match2.MatchID);
        Assert.True(TestMember.PredictionsToDoTest.Count == 1);
    }

    [Fact]
    public void TestMemberSearchPrediction()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";

        TestMember TestMember = new TestMember(vorname, nachname, email, "1234");
        FootballMatch match1 = new FootballMatch(
            "../../../EM_2024Test.csv",
            1,
            SportsTypes.Football
        );
        FootballPrediction prediction1 = new FootballPrediction(
            TestMember.MemberID,
            match1,
            match1.MatchDate,
            1,
            5
        );
        TestMember.PredictionsDoneTest.Add(prediction1);

        FootballPrediction prediction2 = (FootballPrediction)
            TestMember.SearchPredictionDone(prediction1.PredictionID);
        Assert.True(prediction1 == prediction2);
    }

    [Fact]
    public static void TestCalculateScore()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";

        TestMember TestMember = new TestMember(vorname, nachname, email, "1234");

        FootballMatch match1 = new FootballMatch(
            "../../../EM_2024Test.csv",
            1,
            SportsTypes.Football
        );
        FootballMatch match2 = new FootballMatch(
            "../../../EM_2024Test.csv",
            51,
            SportsTypes.Football
        );

        DateTime expected_date = DateTime.Now;

        FootballPrediction prediction1 = new FootballPrediction(
            TestMember.MemberID,
            match1,
            expected_date,
            5,
            1
        );
        FootballPrediction prediction2 = new FootballPrediction(
            TestMember.MemberID,
            match2,
            expected_date,
            0,
            0
        );

        TestMember.PredictionsDoneTest.Add(prediction1);
        TestMember.PredictionsDoneTest.Add(prediction2);

        TestScore score = new TestScore(ScheduleTypes.EM_2024);
        TestMember.ScoresTest.Add(score);

        TestMember.CalculateScores();
        TestScore test_score = (TestScore)
            TestMember.ScoresTest.Find(s => s.ScoreID == ScheduleTypes.EM_2024);

        uint expected_amount_of_points = 18;
        uint actual_amount_of_points = test_score.AmountOfPointsTest;

        Assert.True(
            expected_amount_of_points == actual_amount_of_points,
            $"\tExpected: {expected_amount_of_points}\n\tActual: {actual_amount_of_points}"
        );
    }

    // [Fact]
    // public void TestCalculateScores()
    // {
    //     string vorname = "Maria";
    //     string nachname = "Magdalena";
    //     string email = "maria.magdalena@online.de";

    //     TestMember TestMember = new TestMember(vorname, nachname, email);

    //     Schedule<Match> schedule = new Schedule<Match>(
    //         "../../../EM_2024Test.csv",
    //         SportsTypes.Football,
    //         ScheduleTypes.EM_2024
    //     );

    //     TestMember.AddParticipatingSchedule(schedule, ScheduleTypes.EM_2024);

    //     FootballMatch match1 = new FootballMatch(
    //         "../../../EM_2024Test.csv",
    //         1,
    //         SportsTypes.Football
    //     );
    //     FootballMatch match2 = new FootballMatch(
    //         "../../../EM_2024Test.csv",
    //         51,
    //         SportsTypes.Football
    //     );

    //     TestMember.PredictionsToDoTest.Add(match1); //<- simulated example szenario
    //     TestMember.PredictionsToDoTest.Add(match2);

    //     //TestMember.AddPredictionToDo(); //<- real example szenario
    //     TestMember.ConvertPredictionsDone(match1.MatchID, 5, 1);
    //     TestMember.ConvertPredictionsDone(match2.MatchID, 2, 1);
    //     Assert.True(TestMember.PredictionsDoneTest.Count == 2);
    //
    //     Assert.True(TestMember.ScoresTest.First().AmountOfPointsTest == 36); //36, because 2x18 poin for perfect predictions
    //     Assert.True(TestMember.PredictionsDoneTest.Count == 0);
    //     Assert.True(TestMember.ArchivedPredictionsTest.Count == 2);
    //     Assert.True(TestMember.ScoresTest.Count == 1);
    // }
}
