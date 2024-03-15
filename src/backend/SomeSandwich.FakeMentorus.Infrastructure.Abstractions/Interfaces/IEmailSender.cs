namespace SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;

/// <summary>
/// Contains methods to send emails.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Send an email.
    /// </summary>
    /// <param name="text">Data for an email body.</param>
    /// <param name="subject">Subject of email.</param>
    /// <param name="to">List of recipient's email addresses.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    Task SendEmailAsync(string text, string subject, IList<string> to, CancellationToken cancellationToken);
}
