public class PredictionGameTest
{
    private static IMatchFactory<FootballMatch?> footballMatchFactory = new FootballMatchFactory();

    [Fact]
    public void SendDailyEmailTest()
    {
        CSVReader<FootballMatch?, FootballPrediction?>.SetMatchFactory(footballMatchFactory);

        EmailService HeutigeMail = new EmailService();
        List<ScheduleTypes> scheduleTypes = new List<ScheduleTypes>();
        scheduleTypes.Add(ScheduleTypes.EM_2024);
        PredictionGame predictionGame = new PredictionGame(HeutigeMail);

        Schedule<Match?> schedule = new Schedule<Match?>(
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
        Assert.Equal(2, member1.GetPredictionsToDo().Count);
        member1.PredictionsToDoTest.Add(match1);
        member1.PredictionsToDoTest.Add(match2);
        //member1.ConvertPredictionsDone(member1.GetPredictionsToDo()[0].MatchID, 3, 1);
        member1.ConvertPredictionsDone(match1, 5, 1);
        member1.ConvertPredictionsDone(match2, 2, 1);
        Assert.Equal(2, member1.PredictionsDoneTest.Count);

        member1.CalculateScores();
        //Console.WriteLine(member1.SearchScore(ScheduleTypes.EM_2024).AmountOfPoints);
        //Assert.True(member1.GetArchivedPredictions().Count == 3);
        Assert.True(member1.GetScores().Count() == 1);
        Assert.True(member1.GetScores().First().ScoreID == ScheduleTypes.EM_2024);

        //predictionGame.Register(member2);


        predictionGame.SendDailyEmail();
    }
}
