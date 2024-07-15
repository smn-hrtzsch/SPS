using System;
using System.Collections.Generic;

///\brief Represents a member participating in the Sport Prediction System (SPS).
public class Member<T, M>
    where T : Prediction
    where M : Match
{
    ///\brief Gets the unique ID of the member.
    public uint MemberID { get; }

    private static uint MemberIDCounter = 0;

    private string? forename { get; set; }
    private string? surname { get; set; }
    private string EmailAddress { get; set; }
    private List<Schedule<M>> ParticipatingSchedules;
    private List<T> PredictionsToDo;
    private List<Score> Scores;

    /// \brief Initializes a new instance of the <see cref="Member"/> class.
    public Member(string forname, string surname, string EmailAddress)
    {
        this.forename = forename;
        this.surname = surname;
        this.EmailAddress = EmailAddress;
        MemberID = ++MemberIDCounter;
    }

    /// \brief Adds a schedule to the member's list of participating schedules.
    public void AddSchedule(uint ScheduleID)
    {
        // Implementation for adding a schedule
    }

    /// \brief Removes a schedule from the member's list of participating schedules.
    public void RemoveSchedule(uint ScheduleID)
    {
        // Implementation for removing a schedule
    }

    /// \brief Adds a prediction to the member's list of predictions to do.
    public void AddPrediction(uint PredictionID)
    {
        // Implementation for adding a prediction
    }

    /// \brief Removes a prediction from the member's list of predictions to do.
    public void RemovePrediction(uint PredictionID)
    {
        // Implementation for removing a prediction
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
