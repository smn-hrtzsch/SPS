using System.Reflection.Metadata.Ecma335;

public class TestMember : Member<Prediction, Match>
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

    public List<Score> ScoresTest
    {
        get { return Scores; }
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
        CSVReader<FootballMatch>.SetMatchFactory(footballMatchFactory);
        Schedule<Match> schedule = new Schedule<Match>(
            "../../../EM_2024Test.csv",
            SportsTypes.Football,
            ScheduleTypes.EM_2024
        );

        TestMember.AddParticipatingSchedule(schedule, ScheduleTypes.EM_2024);
        Assert.True(TestMember.ParticipatingSchedulesTest.Count == 1);
    }

    [Fact]
    public void TestMemberRemoveParticipatingSchedule()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";
        string password = "1234";

        TestMember TestMember = new TestMember(vorname, nachname, email, password);
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
        string password = "1234";

        TestMember TestMember = new TestMember(vorname, nachname, email, password);
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
    public void TestMemberSearchPrediction() //TODO
    { }
}
