using System.Reflection.Metadata.Ecma335;

public class TestMember : Member
{

    public TestMember(string forename, string surname, string emailaddress) : base(forename, surname, emailaddress)
    {}
    public List<Schedule> ParticipatingSchedulesTest
    {
        get { return ParticipatingSchedules; }
    }

    public List<Match> PredictionsToDoTest 
    {
        get {return PredictionsToDo; }
    }

    public List<Prediction> PredictionsDoneTest
    {
        get {return PredictionsDone;}
    }

    public List<Score> ScoresTest
    {
        get {return Scores; }
    }
}
public class MemberTest
{
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
        Schedule  schedule = new Schedule("../../../EM_2024Test.csv", SportsTypes.Football, ScheduleTypes.EM_2024);

        TestMember.AddParticipatingSchedule(schedule);
        Assert.True(TestMember.ParticipatingSchedulesTest.Count == 1);
    }

    public void TestMemberRemoveParticipatingSchedule()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";

        TestMember TestMember = new TestMember(vorname, nachname, email);
        Schedule  schedule = new Schedule("../../../EM_2024Test.csv", SportsTypes.Football, ScheduleTypes.EM_2024);

        TestMember.AddParticipatingSchedule(schedule);
        TestMember.RemoveParticipatingSchedule(ScheduleTypes.EM_2024);
        Assert.True(TestMember.ParticipatingSchedulesTest.Count == 0);
    }

    public void TestMemberAddPredictionToDo()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";

        TestMember TestMember = new TestMember(vorname, nachname, email);
        Schedule  schedule = new Schedule("../../../EM_2024Test.csv", SportsTypes.Football, ScheduleTypes.EM_2024);

        TestMember.AddParticipatingSchedule(schedule);
        TestMember.AddPredictionToDo();
        Assert.True(TestMember.PredictionsToDoTest.Count == schedule.GetMatchesOnDay().Count, "GetMatchesOnDay() or the Member Reference does not work");
    }

    public void TestMemberRemovePredictionToDo()
    {
        string vorname = "Maria";
        string nachname = "Magdalena";
        string email = "maria.magdalena@online.de";

        TestMember TestMember = new TestMember(vorname, nachname, email);

        FootballMatch match1 = new FootballMatch("../../../EM_2024Test.csv", 1);
        FootballMatch match2 = new FootballMatch("../../../EM_2024Test.csv", 51);

        TestMember.PredictionsToDoTest.Add(match1);
        TestMember.PredictionsToDoTest.Add(match2);

        Assert.True(TestMember.PredictionsToDoTest.Count == 2);
        TestMember.RemovePredictionToDo(match2.MatchID);
        Assert.True(TestMember.PredictionsToDoTest.Count == 1);
    }

    public void TestMemberSearchPrediction() //TODO
    {}
}