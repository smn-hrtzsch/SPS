using System;
using System.Collections.Generic;

///\brief Represents a member participating in the Sport Prediction System (SPS).
public class Member
{
    ///\brief Gets the unique ID of the member.
    public uint MemberID { get; }
    private string? forename { get; set; }
    private string? surname { get; set; }
    private string EmailAddress { get; set; }
    private string password { get; set; }
    private List<Schedule> ParticipatingSchedules;
    private List<Match> PredictionsToDo { get; }
    private List<Prediction> PredictionsDone { get; }
    private List<Score> Scores;

    /// \brief Initializes a new instance of the <see cref="Member"/> class.
    public Member(string forname, string surname, string emailaddress)
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
        foreach(var schedule in ParticipatingSchedules)
        {
            if(schedule.ScheduleID == schedule_type)
            {
                ParticipatingSchedules.Remove(schedule);
            }
            else
            {
                throw new InvalidOperationException("Schedule Typ is not included in 'ParticipatingSchedules'-List");
            }
        }
    }

    /// \brief Adds a prediction to the member's list of predictions to do.
    public void AddPredictionToDo()
    {
        foreach(Schedule schedule in ParticipatingSchedules)
        {
            List<Match> MatchesOnDay = schedule.GetMatchesOnDay();

            foreach(Match match in MatchesOnDay)
            {
                PredictionsToDo.Add(match);
            }
        }   
    }

    /// \brief Removes a prediction from the member's list of predictions to do.
    public void RemovePredictionToDo(uint MatchID) //remove specific match (if needed, for example for debugging and testing)
    {
        foreach(var match in PredictionsToDo)
        {
            if(match.MatchID == MatchID)
            {
                PredictionsToDo.Remove(match);
            }
        }
        
    }

    /// \brief Searches for a specific prediction in the member's list.
    /// \return The prediction if found, otherwise null.
    public Prediction SearchPrediction(uint PredictionID)
    {
        // Implementation for searching a prediction
        return null;
    }

    /// \brief Adds a score to the member's list of scores.
    public void AddScore(ScheduleTypes PredictedSchedule)
    {
        // Implementation for adding a score
    }

    /// \brief Updates a score in the member's list of scores.
    public void UpdateScore(ScheduleTypes PredictedSchedule, Prediction prediction)
    {
        // Implementation for updating a score
    }
}
