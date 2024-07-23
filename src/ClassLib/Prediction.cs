using System;
using System.Security.Cryptography.X509Certificates;

public abstract class Prediction
{
    public uint PredictionID { get; }
    public uint MemberID { get; }
    public Match PredictedMatch { get; }
    protected DateTime PredictionDate { get; set; }

    public Prediction(uint member_id, Match predicted_match, DateTime predictionDate)
    {
        MemberID = member_id;
        PredictedMatch = predicted_match;
        PredictionDate = predictionDate;
        PredictionID = (uint)GetHashCode();
    }

    public DateTime GetPredictionDate() => PredictionDate;

    public bool ValidatePrediction()
    {
        return PredictionDate < PredictedMatch.MatchDate;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PredictedMatch.GetHashCode(), PredictionDate);
    }

    public override string ToString()
    {
        return $"{PredictionID}, {MemberID}, {PredictedMatch.ToString()}, {PredictionDate}";
    }
}
