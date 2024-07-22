using System;
using System.Formats.Asn1;
using System.Xml.Serialization;
using System.Xml.XPath;

public class Score
{
    public ScheduleTypes ScoreID { get; }
    protected uint AmountOfPoints { get; set; }

    public Score(ScheduleTypes predicted_schedule)
    {
        ScoreID = predicted_schedule;
        AmountOfPoints = 0;
    }

    public void IncrementAmountOfPoints(uint points)
    {
        AmountOfPoints += points;
    }

    public uint CalculateFootballScore(FootballPrediction prediction)
    {
        uint ScoreForPrediction = 0; // wenn nichts der unten genannten Ereignisse eintrifft gibt es für die Prediction keine Punkte
        if (
            prediction.PredictionHome > prediction.PredictionAway
            && prediction.PredictedMatch.Team1Won()
        )
        {
            ScoreForPrediction += 5; // wenn der Sieg der Heimmannschaft richtig getippt wurde, gibt es 5 Punkte
            if (
                prediction.PredictionHome - prediction.PredictionAway
                == prediction.PredictedMatch.ResultTeam1 - prediction.PredictedMatch.ResultTeam2
            )
            {
                ScoreForPrediction += 3; // bei richtig getippter Tordifferenz gibt es 3 extra Punkte
            }
        }
        else if (
            prediction.PredictionHome < prediction.PredictionAway
            && prediction.PredictedMatch.Team2Won()
        )
        {
            ScoreForPrediction += 5; // wenn der Sieg der Auswärtsmannschaft richtig getippt wurde, gibt es 5 Punkte
            if (
                prediction.PredictionAway - prediction.PredictionHome
                == prediction.PredictedMatch.ResultTeam2 - prediction.PredictedMatch.ResultTeam1
            )
            {
                ScoreForPrediction += 3; // bei richtig getippter Tordifferenz gibt es 3 extra Punkte
            }
        }
        if (
            prediction.PredictionHome == prediction.PredictionAway
            && prediction.PredictedMatch.Tie()
        )
        {
            ScoreForPrediction += 5; // wenn ein Unentschieden richtig getippt wurde, gibt es 5 Punkte für die
        }
        if (
            prediction.PredictedMatch.ResultTeam1 == prediction.PredictionHome
            && prediction.PredictedMatch.ResultTeam2 == prediction.PredictionAway
        )
        {
            ScoreForPrediction += 10; // wenn Ergebnis exakt richtig getippt wurde, gibt es 10 extra Punkte (nur, wenn es kein )
        }
        return ScoreForPrediction;
    }

    public override string ToString()
    {
        return $"{AmountOfPoints}";
    }
}
