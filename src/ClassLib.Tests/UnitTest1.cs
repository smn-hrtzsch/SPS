namespace ClassLib.Tests;

using ClassLib;

public class UnitTest1
{
    [Fact]
    public static void TestMail()
    {
        EmailService HeutigeMail = new EmailService();
        HeutigeMail.SendEmail(
            "artimmeyer@gmail.com",
            "sportspredictionsystem@gmail.com",
            "test email",
            "Hallo Artim. Diese Mail wird per C# gesendet!"
        );
    }
}
