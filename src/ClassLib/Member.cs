using System;
using System.Collections.Generic;

/// <summary>
/// Represents a member participating in the Sport Prediction System (SPS).
/// </summary>
public class Member
{
    /// <summary>
    /// Gets the unique ID of the member.
    /// </summary>
    public uint MemberID { get; }

    private static uint MemberIDCounter = 0;

    private string? forename { get; set; }
    private string? surname { get; set; }
    private string EmailAdress { get; set; }
    private List<Schedules> ParticipatingSchedules;
    private List<Prediction> PredictionsToDo;
    private List<Score> Scores;

    /// <summary>
    /// Initializes a new instance of the <see cref="Member"/> class.
    /// </summary>
    public Member(string forname, string surname, string emailAdress)
    {
        this.forename = forname;
        this.surname = surname;
        this.EmailAdress = emailAdress;
        MemberID = ++MemberIDCounter;
        ParticipatingSchedules = new List<Schedules>();
        PredictionsToDo = new List<Prediction>();
        Scores = new List<Score>();
    }

    /// <summary>
    /// Adds a schedule to the member's list of participating schedules.
    /// </summary>
    public void AddSchedule(uint ScheduleID)
    {
        // Implementation for adding a schedule
    }

    /// <summary>
    /// Removes a schedule from the member's list of participating schedules.
    /// </summary>
    public void RemoveSchedule(uint ScheduleID)
    {
        // Implementation for removing a schedule
    }

    /// <summary>
    /// Adds a prediction to the member's list of predictions to do.
    /// </summary>
    public void AddPrediction(uint PredictionID)
    {
        // Implementation for adding a prediction
    }

    /// <summary>
    /// Removes a prediction from the member's list of predictions to do.
    /// </summary>
    public void RemovePrediction(uint PredictionID)
    {
        // Implementation for removing a prediction
    }

    /// <summary>
    /// Searches for a specific prediction in the member's list.
    /// </summary>
    /// <returns>The prediction if found, otherwise null.</returns>
    public Prediction SearchPrediction(uint PredictionID)
    {
        // Implementation for searching a prediction
        return null;
    }

    /// <summary>
    /// Adds a score to the member's list of scores.
    /// </summary>
    public void AddScore(ScheduleTypes PredictedSchedule)
    {
        // Implementation for adding a score
    }

    /// <summary>
    /// Updates a score in the member's list of scores.
    /// </summary>
    public void UpdateScore(ScheduleTypes PredictedSchedule, Prediction prediction)
    {
        // Implementation for updating a score
    }
}
