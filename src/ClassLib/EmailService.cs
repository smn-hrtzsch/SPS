using System;

/// <summary>
/// Provides functionality to send emails using SMTP.
/// </summary>
class EmailService
{
    /// <summary>
    /// Gets or sets the SMTP server address.
    /// </summary>
    private string SmtpServer { get; set; }

    /// <summary>
    /// Gets or sets the SMTP server port.
    /// </summary>
    private int SmtpPort { get; set; }

    /// <summary>
    /// Gets or sets the username for SMTP authentication.
    /// </summary>
    private string Username { get; set; }

    /// <summary>
    /// Gets or sets the password for SMTP authentication.
    /// </summary>
    private string Password { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailService"/> class.
    /// </summary>
    /// <param name="smtpServer">The SMTP server address.</param>
    /// <param name="smtpPort">The SMTP server port.</param>
    /// <param name="username">The username for SMTP authentication.</param>
    /// <param name="password">The password for SMTP authentication.</param>
    public EmailService(string smtpServer, int smtpPort, string username, string password)
    {
        SmtpServer = smtpServer;
        SmtpPort = smtpPort;
        Username = username;
        Password = password;
    }

    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="recipient">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="content">The content of the email.</param>
    public void SendEmail(string recipient, string subject, string content)
    {
        // Implementation for sending email goes here
    }
}
