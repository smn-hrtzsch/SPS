using System;

public class PredictionGame
{
    public uint PredictionGameID { get; }
    private static uint PredictionGameIDCounter = 0;
    private List<Member> Members { get; set; }
    public List<ScheduleTypes> ScheduleTypes { get; }
    private EmailService email_service { get; set; }
    // public PredictionGame (EmailService email_service){}
    // public void Register(Member member){}
    // public void Unsubscribe(int MemberID){}
    // public void SendDailyEmail(){}
}
