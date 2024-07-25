using System;
using System.Net;
using System.Net.Mail;

/// \brief Provides functionality to send emails using SMTP
public class EmailService
{
    /// \brief Gets or sets the SMTP server address.
    private string SmtpServer { get; set; }

    /// \brief Gets or sets the SMTP server port.
    private int SmtpPort { get; set; }

    /// \brief Gets or sets the username for SMTP authentication.
    private string Username { get; set; }

    /// \brief Gets or sets the password for SMTP authentication.
    private string Password { get; set; }

    /// \brief Initializes a new instance of the <see cref="EmailService"/> class.
    /// \param smtpServer The SMTP server address.
    /// \param smtpPort The SMTP server port.
    /// \param username The username for SMTP authentication.
    /// \param password The password for SMTP authentication.
    public EmailService()
    {
        SmtpServer = "smtp.gmail.com";
        SmtpPort = 587;
        Username = "sportspredictionsystem@gmail.com";
        Password = "gkyyxboragzckjbw";
    }

    /// \brief Sends an email.
    /// \param recipient The recipient's email address.
    /// \param subject The subject of the email.
    /// \param content The content of the email.

    public void SendEmail(
        string Recipient,
        string Sender,
        string Subject,
        string Template,
        Dictionary<string, string> placeholders
    )
    {
        foreach (var placeholder in placeholders)
        {
            Template = Template.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
        }

        MailMessage Email = new MailMessage();
        Email.From = new MailAddress(Sender);
        Email.To.Add(Recipient);
        Email.Subject = Subject;
        Email.Body = Template;
        Email.IsBodyHtml = true;

        SmtpClient MailClient = new SmtpClient(SmtpServer, SmtpPort);
        MailClient.EnableSsl = true;
        MailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        MailClient.UseDefaultCredentials = false;
        MailClient.Credentials = new System.Net.NetworkCredential(Username, Password);

        MailClient.Send(Email);
    }
}
