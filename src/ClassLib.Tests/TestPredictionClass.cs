public class TestPrediction : Prediction
{
    public TestPrediction(uint member_id, Match predicted_match, DateTime predictionDate)
        : base(member_id, predicted_match, predictionDate) { }

    public DateTime PredictionDateTest
    {
        get { return PredictionDate; }
    }
}

public class TestPredictionClass
{
    [Fact]
    public static void TestFootballPredictionCtor()
    {
        FootballMatch match2 = new FootballMatch(
            "../../../../../csv-files/EM_2024.csv",
            51,
            SportsTypes.Football
        );
        DateTime expectedDate = DateTime.Now;
        FootballPrediction prediction1 = new FootballPrediction(1, match2, expectedDate, 1, 3);
        TestPrediction testprediction1 = new TestPrediction(1, match2, expectedDate);

        uint expectedID = (uint)
            HashCode.Combine(match2.GetHashCode(), testprediction1.PredictionDateTest);
        string expected_result =
            $"{expectedID}, 1, {match2.ToString()}, {testprediction1.PredictionDateTest}, 1, 3";

        Assert.True(
            expected_result == prediction1.ToString(),
            $"\tExpected:\t{expected_result}\n\tActual:\t\t{prediction1.ToString()}"
        );
    }

    [Fact]
    public static void TestValidatePrediction()
    {
        FootballMatch match2 = new FootballMatch(
            "../../../../../csv-files/EM_2024.csv",
            51,
            SportsTypes.Football
        );
        FootballPrediction prediction2 = new FootballPrediction(1, match2, DateTime.Now, 1, 3);

        Assert.False(prediction2.ValidatePrediction());
    }
}
