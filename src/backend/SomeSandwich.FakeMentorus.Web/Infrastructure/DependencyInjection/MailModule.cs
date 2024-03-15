using System.Net;
using System.Net.Mail;
using SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Interfaces;
using SomeSandwich.FakeMentorus.Infrastructure.Implements;

namespace SomeSandwich.FakeMentorus.Web.Infrastructure.DependencyInjection;

public class MailModule
{
    /// <summary>
    /// Register dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    /// <param name="configuration"></param>
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        var mailSection = configuration.GetSection("Mail");
        if (!int.TryParse(mailSection["Port"] ?? throw new ArgumentNullException("Mail:Port"), out var port))
        {
            throw new Exception();
        }

        if (!bool.TryParse(mailSection["UseSsl"] ?? throw new ArgumentNullException("Mail:UseSsl"), out var enableSsl))
        {
            throw new Exception();
        }

        var smtpClient = new SmtpClient
        {
            Host = mailSection["Host"] ?? throw new ArgumentNullException("Mail:UseSsl"),
            Port = port,
            EnableSsl = enableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                mailSection["Username"] ?? throw new ArgumentNullException("Mail:Username"),
                mailSection["Password"] ?? throw new ArgumentNullException("Mail:Password"))
        };
        services.AddSingleton<IEmailSender, GoogleSmtpSenderService>(_ => new GoogleSmtpSenderService(smtpClient));
    }
}
