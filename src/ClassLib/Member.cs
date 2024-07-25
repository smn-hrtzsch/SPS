using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public interface IMemberData<M, P>
    where M : Match
    where P : Prediction
{
    public string? GetForename();
    public string? GetSurname();
    public string GetEmailAddress();
    public string GetPassword();
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
    public string? GetForename() => Forename;

    public string? GetSurname() => Surname;

    /// \brief Retrieves the email address of the member.
    public string GetEmailAddress() => EmailAddress;

    public string GetPassword() => Password;

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
        PredictionsToDo.Clear();
        foreach (Schedule<M> schedule in ParticipatingSchedules)
        {
            List<M> MatchesOnDay = schedule.GetMatchesOnDay();

            List<M?> AlreadyPredictedMatches = new List<M?>();

            foreach (var prediction in PredictionsDone)
            {
                AlreadyPredictedMatches.Add(prediction.PredictedMatch as M);
                // Console.WriteLine($"In PredictionsDone gefunden: {prediction.PredictedMatch.MatchID} {prediction.PredictedMatch}");
            }
            foreach (var prediction in ArchivedPredictions)
            {
                AlreadyPredictedMatches.Add(prediction.PredictedMatch as M);
                // Console.WriteLine($"In ArchivedPredictions gefunden: {prediction.PredictedMatch.MatchID} {prediction.PredictedMatch}");
            }
            //Console.WriteLine($"Anzahl der bereits getippten Spiele: {AlreadyPredictedMatches.Count}");

            foreach (M match in MatchesOnDay)
            {
                if (!AlreadyPredictedMatches.Contains(match))
                {
                    //Console.WriteLine($"Zu PredictionsToDo hinzugefügt: {match.MatchID} {match}");
                    PredictionsToDo.Add(match);
                }
            }
        }
    }

    /// \brief Removes a match from the member's list of PredictionsToDo.
    public void RemovePredictionToDo(uint MatchID) // remove specific match via MatchID
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
    public void ConvertPredictionsDone(
        M predicted_match,
        byte prediction_home,
        byte prediction_away
    )
    {
        // M? match_to_predict = null;
        // Console.WriteLine($"Anzahl in PredictionsToDo: {PredictionsToDo.Count}");
        // Console.WriteLine("PredictionsToDo:");
        // foreach (var match in PredictionsToDo) {
        //     Console.WriteLine($"{match.MatchID} {match}");
        // }
        // Console.WriteLine($"Match nach dem gesucht wird: {predicted_match.MatchID} {predicted_match}");
        // foreach (var match in PredictionsToDo)
        // {
        //     if (match == predicted_match)
        //     {
        //         match_to_predict = predicted_match;
        //         break;
        //     }
        //     else
        //     {
        //         throw new InvalidOperationException(
        //             "Match was not found in PredictionsToDo. (ConvertPredictionDone())"
        //         );
        //     }
        // }
        if (
            predicted_match != null
            && Prediction.ValidatePredictionDate(DateTime.Now, predicted_match.MatchDate)
        )
        {
            switch (predicted_match.SportsType) //switch case for ctor calls
            {
                case SportsTypes.Football:
                    FootballPrediction predictionDone = new FootballPrediction(
                        MemberID,
                        predicted_match as FootballMatch,
                        DateTime.Now,
                        prediction_home,
                        prediction_away
                    );
                    PredictionsDone.Add(predictionDone as P);
                    break;
            }
            PredictionsToDo.Remove(predicted_match);
        }
        else
        {
            Console.WriteLine(
                "Your Prediction is not valid, because you wanted to predict a match, which has already started."
            );
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
        foreach (var score in Scores)
        {
            switch (score.ScoreID)
            {
                case ScheduleTypes.EM_2024:
                    List<P> predictionsToRemove = new List<P>();
                    foreach (P prediction in PredictionsDone)
                    {
                        if (
                            prediction.PredictedMatch.SportsType == SportsTypes.Football
                            && prediction.PredictedMatch.ResultTeam1 != null
                            && prediction.PredictedMatch.ResultTeam2 != null
                        )
                        {
                            uint ScoreForPrediction = score.CalculateFootballScore(
                                prediction as FootballPrediction
                            );
                            score.IncrementAmountOfPoints(ScoreForPrediction);
                            predictionsToRemove.Add(prediction);
                        }
                        else
                        {
                            break;
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

    public Score SearchScore(ScheduleTypes schedule_type)
    {
        Score? searchedScore = null;

        foreach (var score in Scores)
        {
            if (score.ScoreID == schedule_type)
            {
                searchedScore = score;
            }
            else
            {
                throw new InvalidOperationException(
                    "There exists no score for searched schedule type."
                );
            }
        }

        return searchedScore;
    }
}
