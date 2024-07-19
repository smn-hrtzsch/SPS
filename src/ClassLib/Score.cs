using System;
using System.Formats.Asn1;

public class Score
{
    public uint ScoreID { get; }
    private ScheduleTypes PredictedSchedule { get; }
    private uint AmountOfPoints { get; set; }

    public Score(ScheduleTypes predicted_schedule)
    {
        PredictedSchedule = predicted_schedule;
        AmountOfPoints = 0;
        ScoreID = (uint)GetHashCode();
    }

    public int CalculateScore(ScheduleTypes PredictedSchedule, Prediction prediction)
    {
        return 0;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PredictedSchedule, AmountOfPoints);
    }
}
