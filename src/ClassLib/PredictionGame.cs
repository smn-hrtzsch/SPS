using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

/// \brief Represents a prediction game in the Sport Prediction System (SPS).
public class PredictionGame
{
    /// \brief Gets the unique ID of the prediction game.

    public uint PredictionGameID { get; }

    private static uint PredictionGameIDCounter = 0;

    private EmailService email_service { get; set; }

    public List<Member<Match, Prediction>> Members { get; set; }

    public List<ScheduleTypes> ScheduleTypesList { get; }

    /// \brief Initializes a new instance of the <see cref="PredictionGame"/> class.

    public PredictionGame(EmailService emailService)
    {
        email_service = emailService;
        PredictionGameID = (uint)GetHashCode();
        Members = new List<Member<Match, Prediction>>();
        ScheduleTypesList = GetAllScheduleTypes();
    }

    private List<ScheduleTypes> GetAllScheduleTypes() =>
        Enum.GetValues(typeof(ScheduleTypes)).Cast<ScheduleTypes>().ToList();

    /// \brief Get a unique Hashcode -> PredictionGameIDCounter necassary for generation
    public override int GetHashCode()
    {
        PredictionGameIDCounter++;
        return HashCode.Combine(email_service, PredictionGameIDCounter);
    }

    /// \brief Registers a new member to the prediction game.

    public void Register(Member<Match, Prediction> member)
    {
        Members.Add(member);
    }

    /// \brief Unsubscribes a member from the prediction game.

    public void Unsubscribe(int MemberID)
    {
        Members.RemoveAll(m => m.MemberID == MemberID);
    }

    /// \brief Sends a daily email to all members with the matches that need to be predicted.

    public void SendDailyEmail()
    {
        //         string TippAndResultTemplate =
        //             @"
        //         <html>
        //     <body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;'>
        //         <h1 style='color: #444;'>Hallo {{forename}},</h1>
        //         <p style='font-size: 16px; color: #666;'>Bereit, deine Erfolge zu feiern? Hier sind die faszinierenden Ergebnisse der letzten Spiele und die Punkte, die du erreicht hast:</p>
        //         <ul style='list-style-type: none; padding: 0;'>
        //             {{results}}
        //         </ul>
        //         <p style='font-size: 16px; color: #666;'>Deine Gesamtpunkte: <strong>{{totalPoints}}</strong></p>
        //         <p style='font-size: 16px; color: #666;'>Bleib am Ball und genieße das Tippspiel. Weiter so und viel Erfolg!</p>

        //         <h2 style='color: #444;'>Bereit für ein aufregendes Spielabenteuer?</h2>
        //         <p style='font-size: 16px; color: #666;'>Hier sind deine exklusiven Tipps für heute:</p>
        //         <ul style='list-style-type: none; padding: 0;'>
        //             {{matches}}
        //         </ul>
        //         <p style='font-size: 16px; color: #666;'>Spüre den Nervenkitzel, gebe deine Tipps ab, und lass dich von deinem Instinkt leiten. Klicke einfach auf diesen <a href='{{link}}' style='color: #ff6347; text-decoration: none;'>magischen Link</a> und tauche ein in die Welt von SportsPredictionSystem.</p>
        //         <p style='font-size: 16px; color: #666;'>Viel Erfolg und genieße den Moment!</p>
        //     </body>
        // </html>";

        string GeneralTippResult =
            @"<html>
            <body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;'>
                <h1 style='color: #444;'>Hallo geehrter SPSMember,</h1>
                <p style='font-size: 16px; color: #666;'>Bereit für ein aufregendes Spielabenteuer? Hier sind die Spiele die heute getippt werden können:</p>
                <ul style='list-style-type: none; padding: 0;'>
                    {{matches}}
                </ul>
                <p style='font-size: 16px; color: #666;'>Spüre den Nervenkitzel, gebe Deine Tipps ab, und lass dich von deinem Instinkt leiten. Klicke einfach auf diesen <a href='{{link}}' style='color: #ff6347; text-decoration: none;'>magischen Link</a> und tauche ein in die Welt von SportsPredictionSystem.</p>
                <p style='font-size: 16px; color: #666;'>Viel Erfolg und genieße den Moment!</p>
            </body>
        </html>";

        foreach (var schedule in ScheduleTypesList)
        {
            foreach (var member in Members)
            {
                string matchesList = string.Empty;
                string resultsList = string.Empty;
                //uint totalPoints = 0;

                foreach (var match in member.GetPredictionsToDo())
                {
                    if (schedule == ScheduleTypes.EM_2024)
                    {
                        matchesList +=
                            $"<li style='font-size: 16px; color: #444; margin: 5px 0;'>{(match as FootballMatch).HomeTeam} vs  {(match as FootballMatch).AwayTeam} am {(match as FootballMatch).MatchDate.ToString("dd.MM.yyyy HH:mm")}</li>";
                    }
                }
                // foreach (var score in member.GetScores())
                // {
                //     switch (score.ScoreID)
                //     {
                //         case ScheduleTypes.EM_2024:
                //             foreach (Prediction prediction in member.GetArchivedPredictions())
                //             {
                //                 DateTime predictionDate = prediction.GetPredictionDate();
                //                 DateTime yesterday = DateTime.Now.AddDays(-1);
                //                 DateTime yesterday_yesterday = DateTime.Now.AddDays(-1);

                //                 if(predictionDate.Day == DateTime.Now.Day || predictionDate.Day == yesterday.Day || predictionDate.Day == yesterday_yesterday.Day) //todays predictions, yesterdays predictions and the predictions two days a
                //                 {

                //                 uint points = score.CalculateFootballScore(
                //                     prediction as FootballPrediction
                //                 );
                //                 totalPoints = member.SearchScore(ScheduleTypes.EM_2024).AmountOfPoints;
                //                 resultsList +=
                //                     $"<li style='font-size: 16px; color: #444; margin: 5px 0;'>{(prediction.PredictedMatch as FootballMatch).HomeTeam} vs {(prediction.PredictedMatch as FootballMatch).AwayTeam} am {(prediction.PredictedMatch as FootballMatch).MatchDate.ToString("dd.MM.yyyy HH:mm")}: {(prediction.PredictedMatch as FootballMatch).ResultTeam1} - {(prediction.PredictedMatch as FootballMatch).ResultTeam2} (Deine Vorhersage: {(prediction as FootballPrediction).PredictionHome} -  {(prediction as FootballPrediction).PredictionAway}, Punkte: {points})</li>";
                //                 }
                //             }
                //             break;

                //         default:
                //             break;
                //     }
                // }
                Dictionary<string, string> TippDic = new Dictionary<string, string>
                {
                    // { "forename", member.GetForename() },
                    { "matches", matchesList },
                    { "link", "https://github.com/smn-hrtzsch/SPS" },
                    //{ "results", resultsList },
                    //{ "totalPoints", totalPoints.ToString() }
                };
                email_service.SendEmail(
                    member.GetEmailAddress(),
                    "sportspredictionsystem@gmail.com",
                    "Klick hier um zu Tippen! ;D",
                    GeneralTippResult,
                    TippDic
                );
            }
        }
    }
}
