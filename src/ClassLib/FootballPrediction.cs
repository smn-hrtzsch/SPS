using System;

public class FootballPrediction : Prediction
{
    public string? HomeTeam { get; }
    public string? AwayTeam { get; }
    public byte PredictionHome { get; protected set; }
    public byte PredictionAway { get; protected set; }

    public FootballPrediction(
        uint member_id,
        FootballMatch football_match,
        DateTime predictionDate,
        byte prediction_home,
        byte prediction_away
    )
        : base(member_id, football_match, predictionDate)
    {
        PredictionHome = prediction_home;
        PredictionAway = prediction_away;
        HomeTeam = football_match.HomeTeam;
        AwayTeam = football_match.AwayTeam;
    }

    public FootballPrediction(
        uint prediction_id,
        uint member_id,
        FootballMatch football_match,
        DateTime predictionDate,
        byte prediction_home,
        byte prediction_away
    )
        : base(prediction_id, member_id, football_match, predictionDate)
    {
        PredictionHome = prediction_home;
        PredictionAway = prediction_away;
        HomeTeam = football_match.HomeTeam;
        AwayTeam = football_match.AwayTeam;
    }

    public void ChangePrediction(byte? NewPredictionHome, byte? NewPredictionAway)
    {
        PredictionDate = DateTime.Now;
        if (ValidatePrediction())
        {
            if (NewPredictionHome != null)
            {
                PredictionHome = (byte)NewPredictionHome;
            }
            if (NewPredictionAway != null)
            {
                PredictionAway = (byte)NewPredictionAway;
            }
        }
    }

    public override string ToString()
    {
        return base.ToString() + $", {PredictionHome}, {PredictionAway}";
    }
}
