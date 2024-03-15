using System.Net.Mail;
using Saritasa.Tools.Domain.Exceptions;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

namespace SomeSandwich.FakeMentorus.Infrastructure.Implements;

/// <summary>
/// Uses Google SMTP sender to send emails. Generally used in application.
/// </summary>
public class GoogleSmtpSenderService : IEmailSender
{
    private readonly Saritasa.Tools.Emails.IEmailSender emailSender;
    private readonly string fromEmail;

    /// <summary>
    /// Constructor.
    /// </summary>
    public GoogleSmtpSenderService(SmtpClient smtpClient, string fromEmail = "no-reply@example.com")
    {
        emailSender = new Saritasa.Tools.Emails.SmtpClientEmailSender(smtpClient);
        this.fromEmail = fromEmail;
    }

    /// <inheritdoc />
    public async Task SendEmailAsync(string text, string subject, IList<string> to, CancellationToken cancellationToken)
    {
        var mailMessage = new MailMessage { From = new MailAddress(fromEmail) };

        if (to == null || !to.Any())
        {
            throw new DomainException("List of email recipients is empty.");
        }

        try
        {
            foreach (var email in to)
            {
                mailMessage.To.Add(email);
            }
        }
        catch (FormatException ex)
        {
            throw new DomainException("Invalid email format.", ex);
        }

        mailMessage.Subject = subject;

        mailMessage.Body = text.Trim();
        mailMessage.IsBodyHtml = mailMessage.Body.StartsWith("<") && mailMessage.Body.EndsWith(">");

        await emailSender.SendAsync(mailMessage, cancellationToken);
    }
}
