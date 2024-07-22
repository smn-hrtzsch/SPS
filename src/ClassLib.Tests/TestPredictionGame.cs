public class PredictionGameTest
{
    private static IMatchFactory<FootballMatch> footballMatchFactory = new FootballMatchFactory();

    [Fact]
    public void SendDailyEmailTest()
    {
        CSVReader<FootballMatch>.SetMatchFactory(footballMatchFactory);

        EmailService HeutigeMail = new EmailService();
        List<ScheduleTypes> scheduleTypes = new List<ScheduleTypes>();
        scheduleTypes.Add(ScheduleTypes.EM_2024);
        PredictionGame predictionGame = new PredictionGame(HeutigeMail);

        Schedule<Match> schedule = new Schedule<Match>(
            "../../../EM_2024Test.csv",
            SportsTypes.Football,
            ScheduleTypes.EM_2024
        );

        TestMember member1 = new TestMember(
            "Artim",
            "Meyer",
            "Artim-Werner.Meyer@student.tu-freiberg.de",
            "1234"
        );
        //MemberData member2 = new MemberData("Simon", "Hörtzsch", "Simon.Hoertzsch@student.tu-freiberg.de");

        predictionGame.Register(member1);

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

        member1.AddParticipatingSchedule(schedule, ScheduleTypes.EM_2024);
        member1.AddPredictionToDo();
        member1.PredictionsToDoTest.Add(match1);
        member1.PredictionsToDoTest.Add(match2);
        member1.ConvertPredictionsDone(match1.MatchID, 5, 1);
        member1.ConvertPredictionsDone(match2.MatchID, 2, 1);

        member1.CalculateScores();
        Assert.True(member1.GetArchivedPredictions().Count == 2);
        Assert.True(member1.GetScores().Count() == 1);
        Assert.True(member1.GetScores().First().ScoreID == ScheduleTypes.EM_2024);

        //predictionGame.Register(member2);


        predictionGame.SendDailyEmail();
    }
}
