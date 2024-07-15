using System;

public abstract class Prediction
{
    public uint PredictionID { get; }
    private static uint PredictionIDCounter = 0;
    public uint MemberID { get; }
    public uint MatchID { get; }
    public DateTime PredictionDate { get; }

    public Prediction(uint MemberID, uint MatchID)
    {
        // code
    }

    public bool ValidatePrediction()
    {
        // code
    }
}
