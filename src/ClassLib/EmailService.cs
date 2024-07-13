using System;

class EmailService
{
    private string SmtpServer { get; set; }
    private int SmtpPort { get; set; }
    private string Username { get; set; }
    private string Password { get; set; }

    public EmailService(string SmtpServer, int SmtpPort, string Username, string Password) { }

    public void SendEmail(string recipient, string subject, string content) { }
}
