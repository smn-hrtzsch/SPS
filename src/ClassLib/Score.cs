using System;

public class Score {
    public uint ScoreID {get;}
    private static uint ScoreIDCounter = 0;
    private ScheduleTypes PredictedSchedule {get;}
    private uint AmountOfPoints {get; set;}

    public Score(ScheduleTypes PredictedSchedule) {
        // code
    }

    public CalculateScore(ScheduleTypes PredictedSchedule, Prediction prediction) {
        // code
    }
}