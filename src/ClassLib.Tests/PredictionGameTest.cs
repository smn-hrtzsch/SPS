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
        PredictionGame predictionGame = new PredictionGame(HeutigeMail, scheduleTypes);

        Schedule<Match> schedule = new Schedule<Match>(
            "../../../EM_2024Test.csv",
            SportsTypes.Football,
            ScheduleTypes.EM_2024
        );

        TestMember member1 = new TestMember("Artim", "Meyer", "artimmeyer@gmail.com ");
        //TestMember member2 = new TestMember("Artim Werner", "Meyer","Artim-Werner.Meyer@student.tu-freiberg.de");

        predictionGame.Register(member1);

        FootballMatch match1 = new FootballMatch("../../../EM_2024Test.csv", 1, SportsTypes.Football);
        FootballMatch match2 = new FootballMatch("../../../EM_2024Test.csv", 51, SportsTypes.Football);
        FootballPrediction prediction1 = new FootballPrediction(member1.MemberID, match1, match1.MatchDate, 5,1);
        FootballPrediction prediction2 = new FootballPrediction(member1.MemberID, match2, match2.MatchDate, 2,1);
        
        member1.AddParticipatingSchedule(schedule, ScheduleTypes.EM_2024);
        member1.AddPredictionToDo();
        member1.PredictionsDoneTest.Add(prediction1);
        member1.PredictionsDoneTest.Add(prediction2);
        member1.CalculateScores();
        Assert.True(member1.ArchivedPredictionsTest.Count == 2);
        Assert.True(member1.ScoresTest.Count() == 1);
        Assert.True(member1.ScoresTest.First().ScoreID == ScheduleTypes.EM_2024);
        
        //predictionGame.Register(member2);


        predictionGame.SendDailyEmail(EmailTypes.ResultTemplate);
    }
}