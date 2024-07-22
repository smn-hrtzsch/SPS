using System.Reflection.Metadata.Ecma335;

public class TestMember : Member<Prediction, Match>
{
    public TestMember(string forename, string surname, string emailaddress)
        : base(forename, surname, emailaddress) { }

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

    public List<Score> ScoresTest
    {
        get { return Scores; }
    }

    public List<Prediction> ArchivedPredictionsTest
    {
        get { return ArchivedPredictions; }
    }
}

public class TestScores : Score
{
    public TestScores(ScheduleTypes predicted_schedule)
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

        TestMember TestMember = new TestMember(vorname, nachname, email);
        Assert.True(TestMember.MemberID == (uint)TestMember.GetHashCode());
    }

    [Fact]
    public void TestMemmberAddParticipatingSchedule()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";

        TestMember TestMember = new TestMember(vorname, nachname, email);
        CSVReader<FootballMatch>.SetMatchFactory(footballMatchFactory);
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

        TestMember TestMember = new TestMember(vorname, nachname, email);
        CSVReader<FootballMatch>.SetMatchFactory(footballMatchFactory);
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

        TestMember TestMember = new TestMember(vorname, nachname, email);
        CSVReader<FootballMatch>.SetMatchFactory(footballMatchFactory);
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

        TestMember TestMember = new TestMember(vorname, nachname, email);

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
    public void TestMemberSearchPrediction() //TODO
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";

        TestMember TestMember = new TestMember(vorname, nachname, email);
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
