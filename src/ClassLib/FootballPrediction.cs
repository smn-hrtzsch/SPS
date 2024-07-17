using System;

public class FootballPrediction : Prediction
{
    private byte PreditcionHome { get; set; }
    private byte PredictionAway { get; set; }

    public FootballPrediction(uint MemberID, uint MatchID, byte PredictionHome, byte PredictionAway)
        : base(MemberID, MatchID)
    {
        // code
    }

    public void ChangePrediction(
        uint? NewPredictionHome,
        uint? NewPredictionAway,
        uint PredictionID
    )
    {
        // code
    }
}
