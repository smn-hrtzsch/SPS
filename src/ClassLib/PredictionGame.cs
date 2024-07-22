using System;
using System.Collections.Generic;

/// <summary>
/// Represents a prediction game in the Sport Prediction System (SPS).
/// </summary>
public class PredictionGame
{
    /// <summary>
    /// Gets the unique ID of the prediction game.
    /// </summary>
    public uint PredictionGameID { get; }

    private static uint PredictionGameIDCounter = 0;

    public List<Member<Prediction, Match>> Members { get; }

    public List<ScheduleTypes> ScheduleTypes { get; private set;}

    private EmailService email_service { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PredictionGame"/> class.
    /// </summary>
    public PredictionGame(EmailService emailService)
    {
        // ChatGPT Vorschlag -->  PredictionGameID = ++PredictionGameIDCounter;
        //   Members = new List<Member>();
        //   ScheduleTypes = new List<ScheduleTypes>();
        //   email_service = emailService;
    }

    /// <summary>
    /// Registers a new member to the prediction game.
    /// </summary>
    public void Register(Member<Prediction, Match> member)
    {
        // ChatGPT Vorschlag --> Members.Add(member);
    }

    /// <summary>
    /// Unsubscribes a member from the prediction game.
    /// </summary>
    public void Unsubscribe(int MemberID)
    {
        // ChatGPT Vorschlag --> Members.RemoveAll(m => m.MemberID == MemberID);
    }

    /// <summary>
    /// Sends a daily email to all members with the matches that need to be predicted.
    /// </summary>
    public void SendDailyEmail()
    {
        foreach (var member in Members)
        {
            // Implementation for sending daily email
        }
    }
}
