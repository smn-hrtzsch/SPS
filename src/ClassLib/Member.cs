using System;
using System.Collections.Generic;

///\brief Represents a member participating in the Sport Prediction System (SPS).
public class Member
{
    ///\brief Gets the unique ID of the member.
    public uint MemberID { get; }
    protected string? forename { get; set; }
    protected string? surname { get; set; }
    protected string EmailAddress { get; set; }
    protected string password { get; set; }
    protected List<Schedule> ParticipatingSchedules { get; }
    protected List<Match> PredictionsToDo { get; }
    protected List<Prediction> PredictionsDone { get; }
    protected List<Score> Scores;

    /// \brief Initializes a new instance of the <see cref="Member"/> class.
    public Member(string forename, string surname, string emailaddress)
    {
        this.forename = forename;
        this.surname = surname;
        this.EmailAddress = emailaddress;
        this.MemberID = (uint)GetHashCode();
        this.ParticipatingSchedules = new List<Schedule>();
        this.PredictionsToDo = new List<Match>();
        this.PredictionsDone = new List<Prediction>();
        this.Scores = new List<Score>();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(forename, surname, EmailAddress);
    }

    /// \brief Adds a schedule to the member's list of participating schedules.
    public void AddParticipatingSchedule(Schedule schedule)
    {
        ParticipatingSchedules.Add(schedule);
    }

    /// \brief Removes a schedule from the member's list of participating schedules.
    public void RemoveParticipatingSchedule(ScheduleTypes schedule_type)
    {
        foreach (var schedule in ParticipatingSchedules)
        {
            if (schedule.ScheduleID == schedule_type)
            {
                ParticipatingSchedules.Remove(schedule);
            }
            else
            {
                throw new InvalidOperationException(
                    "Schedule Typ is not included in 'ParticipatingSchedules'-List"
                );
            }
        }
    }

    /// \brief Adds a prediction to the member's list of predictions to do.
    public void AddPredictionToDo()
    {
        foreach (Schedule schedule in ParticipatingSchedules)
        {
            List<Match> MatchesOnDay = schedule.GetMatchesOnDay();

            foreach (Match match in MatchesOnDay)
            {
                PredictionsToDo.Add(match);
            }
        }
    }

    /// \brief Removes a prediction from the member's list of predictions to do.
    public void RemovePredictionToDo(uint MatchID) //remove specific match (if needed, for example for debugging and testing)
    {
        foreach (var match in PredictionsToDo)
        {
            if (match.MatchID == MatchID)
            {
                PredictionsToDo.Remove(match);
            }
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

    public void AddPrediction()
    {
        //
    }
    /// \brief Adds a score to the member's list of scores.
    // public void AddScore(Score MatchScore, uint PredictionID)
    // {
    //     foreach(var prediction in PredictionsDone)
    //     {
    //         if(prediction.PredictionID == PredictionID)
    //         {
    //             Scores.Add(MatchScore);
    //         }

    //         else
    //         {
    //             throw new InvalidOperationException("The corresponding Prediction could not found relating to it's PredictionID");
    //         }
    //     }
    // }

    /// \brief Updates a score in the member's list of scores.
    // public void UpdateScore(uint SocreID, string NewScore)
    // {
    //     foreach(var score in Scores)
    //     {
    //         if(score.ScoreID == SocreID)
    //         {
    //             score.AmountOfPoints = NewScore;
    //         }

    //         else
    //         {
    //             throw new InvalidOperationException("The corresponding Score could not be found in the 'Score' List relating to it's ScoreID");
    //         }
    //     }
    // }
}
