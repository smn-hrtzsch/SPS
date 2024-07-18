using System;

public class Score
{
    public uint ScoreID { get; }
    private ScheduleTypes PredictedSchedule { get; }
    public string AmountOfPoints { get; set; } //Zum Beispiel 5:1 oder 6:2 -> kann man dann mit Split umwandeln, siehe GetScoreTeam1 oder GetScoreTeam2

    public Score(uint PredictionID, ScheduleTypes PredictedSchedule, string AmountOfPoints)
    {
        this.ScoreID = PredictionID; //!!!ACHTUNG die ScoreID kennzeichnet die Zugehörigkeit zur Prediction, da die PredictionID und die ScoreID gleich sind (sonst finden wir den Score zur Prediction nicht wieder)
        this.PredictedSchedule = PredictedSchedule;
        this.AmountOfPoints = AmountOfPoints;
    }

    public int CalculateScore(ScheduleTypes PredictedSchedule, Prediction prediction)
    {
        return 0;
    }

    public uint GetScoreTeam1()
    {
        string[] scorearray = AmountOfPoints.Split(':');
        return uint.Parse(scorearray[0]);
    }

    public uint GetScoreTeam2()
    {
        string[] scorearray = AmountOfPoints.Split(':');
        return uint.Parse(scorearray[1]); 
    }
}
