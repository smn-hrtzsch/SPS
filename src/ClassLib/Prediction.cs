using System;
using System.Security.Cryptography.X509Certificates;

public abstract class Prediction
{
    public uint PredictionID { get; }
    public uint MemberID { get; }
    public Match? PredictedMatch { get; }
    public DateTime PredictionDate { get; protected set; }

    public Prediction(uint member_id, Match? predicted_match, DateTime predictionDate)
    {
        MemberID = member_id;
        PredictedMatch = predicted_match;
        PredictionDate = predictionDate;
        PredictionID = (uint)GetHashCode();
    }

    public Prediction(
        uint prediction_id,
        uint member_id,
        Match predicted_match,
        DateTime predictionDate
    )
    {
        PredictionID = prediction_id;
        MemberID = member_id;
        PredictedMatch = predicted_match;
        PredictionDate = predictionDate;
    }

    public DateTime GetPredictionDate() => PredictionDate;

    public static bool ValidatePredictionDate(DateTime prediction_date, DateTime? match_date)
    {
        return prediction_date < match_date;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            PredictedMatch?.GetHashCode(),
            PredictionDate,
            MemberID,
            PredictedMatch?.ToString()
        );
    }

    public override string ToString()
    {
        return $"{PredictionID}, {MemberID}, {PredictedMatch?.ToString()}, {PredictionDate}";
    }
}
