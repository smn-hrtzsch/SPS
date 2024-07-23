using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public interface IMemberData<M, P>
    where M : Match
    where P : Prediction //for further implementation (e.g. premium membership etc.)
{
    public string GetForename();
    public string GetEmailAddress();
    public List<M> GetPredictionsToDo();
    public List<P> GetPredictionsDone();
    public void SetPredictionsDone(List<P> done_predictions);
    public List<P> GetArchivedPredictions();
    public void SetArchivedPredictions(List<P> archived_predictions);
    public List<Score> GetScores();
    public void SetScores(List<Score> scores);
}

///\brief Represents a member participating in the Sport Prediction System (SPS).
public class Member<M, P> : IMemberData<M, P>
    where P : Prediction
    where M : Match
{
    ///\brief Gets the unique ID of the member.
    public uint MemberID { get; }
    protected string? Forename { get; set; }
    protected string? Surname { get; set; }
    protected string EmailAddress { get; }
    protected string Password { get; }

    /// \brief List of Schedules the member chose to participate predicting.
    protected List<Schedule<M>> ParticipatingSchedules { get; }

    /// \brief List of Matches, which need to be predicted on the specific day.
    protected List<M> PredictionsToDo;

    /// \brief List, which contains all Predictions where the match is already predicted, but a score was not calculated yet.
    protected List<P> PredictionsDone;

    /// \brief List, which contains all Predictions where no score must be calculated anymore
    protected List<P> ArchivedPredictions;

    /// \brief List of Scores <summary>
    /// \details There is exactly one score for every schedule the member predicts.
    protected List<Score> Scores;

    /// \brief Retrieves the forename of the member.
    public string GetForename() => Forename;

    /// \brief Retrieves the email address of the member.
    public string GetEmailAddress() => EmailAddress;

    /// \brief Retrieves a copy of the list of matches that need to be predicted.
    public List<M> GetPredictionsToDo() => new List<M>(PredictionsToDo);

    /// \brief Retrieves a copy of the list of the predicitons already done, but not yet archived.
    public List<P> GetPredictionsDone() => PredictionsDone;

    public void SetPredictionsDone(List<P> done_predictions) => PredictionsDone = done_predictions;

    /// \brief Retrieves a copy of the list of archived predictions.
    public List<P> GetArchivedPredictions() => new List<P>(ArchivedPredictions);

    public void SetArchivedPredictions(List<P> archived_predictions) =>
        ArchivedPredictions = archived_predictions;

    /// \brief Retrieves a copy of the scores list.
    public List<Score> GetScores() => new List<Score>(Scores);

    public void SetScores(List<Score> scores) => Scores = scores;

    /// \brief Initializes a new instance of the <see cref="Member"/> class.
    public Member(string forename, string surname, string emailaddress, string password)
    {
        this.Forename = forename;
        this.Surname = surname;
        this.EmailAddress = emailaddress;
        this.Password = password;
        this.MemberID = (uint)GetHashCode();
        this.ParticipatingSchedules = new List<Schedule<M>>();
        this.PredictionsToDo = new List<M>();
        this.PredictionsDone = new List<P>();
        this.ArchivedPredictions = new List<P>();
        this.Scores = new List<Score>();
    }

    public Member(
        uint member_id,
        string forename,
        string surname,
        string emailaddress,
        string password
    )
    {
        MemberID = member_id;
        Forename = forename;
        Surname = surname;
        EmailAddress = emailaddress;
        Password = password;
        ParticipatingSchedules = new List<Schedule<M>>();
        PredictionsToDo = new List<M>();
        PredictionsDone = new List<P>();
        ArchivedPredictions = new List<P>();
        Scores = new List<Score>();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Forename, Surname, EmailAddress);
    }

    /// \brief Adds a schedule to the member's list of participating schedules.
    public void AddParticipatingSchedule(Schedule<M> schedule, ScheduleTypes schedule_type)
    {
        ParticipatingSchedules.Add(schedule);
        Score score = new Score(schedule_type);
        Scores.Add(score);
    }

    /// \brief Removes a schedule from the member's list of participating schedules.
    public void RemoveParticipatingSchedule(ScheduleTypes schedule_type)
    {
        Schedule<M>? scheduleToRemove = null;
        foreach (var schedule in ParticipatingSchedules)
        {
            if (schedule.ScheduleID == schedule_type)
            {
                scheduleToRemove = schedule;
                break;
            }
        }

        if (scheduleToRemove != null)
        {
            ParticipatingSchedules.Remove(scheduleToRemove);
        }
        else
        {
            throw new InvalidOperationException(
                "Schedule Typ is not included in 'ParticipatingSchedules'-List"
            );
        }
    }

    /// \brief Adds a prediction to the member's list of predictions to do.
    public void AddPredictionToDo()
    {
        foreach (Schedule<M> schedule in ParticipatingSchedules)
        {
            List<M> MatchesOnDay = schedule.GetMatchesOnDay();

            foreach (M match in MatchesOnDay)
            {
                PredictionsToDo.Add(match);
            }
        }
    }

    /// \brief Removes a match from the member's list of PredictionsToDo.
    public void RemovePredictionToDo(uint MatchID) // remove specific match (if needed, for example for debugging and testing)
    {
        M? matchToRemove = null;
        foreach (var match in PredictionsToDo)
        {
            if (match.MatchID == MatchID)
            {
                matchToRemove = match;
                break;
            }
        }

        if (matchToRemove != null)
        {
            PredictionsToDo.Remove(matchToRemove);
        }
        else
        {
            throw new InvalidOperationException("Match is not included in 'PredictionsToDo'-List");
        }
    }

    /// \brief If user has predicted a certain match (specified by MatchID), a new prediction will be created (Prediction ctor call) and will be added to the PredictionsDone-list
    public void ConvertPredictionsDone(uint MatchID, byte prediction_home, byte prediction_away)
    {
        M? predictedMatch = null;
        foreach (var match in PredictionsToDo)
        {
            if (match.MatchID == MatchID)
            {
                predictedMatch = match;
                break;
            }
        }
        if (predictedMatch != null)
        {
            switch (predictedMatch.SportsType) //switch case for ctor calls
            {
                case SportsTypes.Football:
                    FootballPrediction predictionDone = new FootballPrediction(
                        MemberID,
                        predictedMatch as FootballMatch,
                        DateTime.Now,
                        prediction_home,
                        prediction_away
                    );
                    PredictionsDone.Add(predictionDone as P);
                    break;
            }
            PredictionsToDo.Remove(predictedMatch);
        }
        else
        {
            throw new InvalidOperationException("Match is not included in 'PredictionsToDo'-List");
        }
    }

    /// \brief Searches for a specific prediction in the member's list.
    /// \return The prediction if found, otherwise null.
    public Prediction SearchPredictionDone(uint PredictionID)
    {
        Prediction? searchedprediction = null;
        foreach (var prediction in PredictionsDone)
        {
            if (prediction.PredictionID == PredictionID)
            {
                searchedprediction = prediction;
            }
            else
            {
                throw new InvalidOperationException(
                    "Prediction is not included in 'PredictionsDone'-List"
                );
            }
        }
        return searchedprediction;
    }

    /// \brief Removes a prediction from the member's list of PredictionsDone.
    public void RemovePredictionsDone(uint PredictionID) // remove specific prediction (if needed, for example for debugging and testing)
    {
        P? predictionToRemove = null;
        foreach (var prediction in PredictionsDone)
        {
            if (prediction.PredictionID == PredictionID)
            {
                predictionToRemove = prediction;
                break;
            }
        }

        if (predictionToRemove != null)
        {
            PredictionsDone.Remove(predictionToRemove);
        }
        else
        {
            throw new InvalidOperationException("Match is not included in 'PredictionsDone-List");
        }
    }

    public void CalculateScores()
    {
        List<P> predictionsToArchive = new List<P>();

        foreach (var score in Scores)
        {
            switch (score.ScoreID)
            {
                case ScheduleTypes.EM_2024:
                    List<P> predictionsToRemove = new List<P>();
                    foreach (P prediction in PredictionsDone)
                    {
                        if (prediction.PredictedMatch.SportsType == SportsTypes.Football)
                        {
                            uint ScoreForPrediction = score.CalculateFootballScore(
                                prediction as FootballPrediction
                            );
                            score.IncrementAmountOfPoints(ScoreForPrediction);
                            predictionsToRemove.Add(prediction);
                        }
                    }
                    foreach (P prediction in predictionsToRemove)
                    {
                        PredictionsDone.Remove(prediction);
                        ArchivedPredictions.Add(prediction);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public override string ToString()
    {
        string mi = $"{MemberID}";
        string fn = $"{Forename}";
        string sn = $"{Surname}";
        string ea = $"{EmailAddress}";
        string pw = $"{Password}";
        return $"{mi};{fn};{sn};{ea};{pw}";
    }
}
