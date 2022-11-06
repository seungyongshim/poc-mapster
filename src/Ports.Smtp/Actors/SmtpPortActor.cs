using FluentEmail.Core;
using MailKit.Net.Smtp;
using MailKit.Security;
using Mapster;
using MimeKit;
using MimeKit.Text;
using Proto;
using Email = SendMailService.Domain.Email;

namespace Ports.Smtp.Actors;

public class SmtpPortActor : IActor
{
    static SmtpPortActor() => TypeAdapterConfig<Email, MailboxAddress>.NewConfig()
            .ConstructUsing(src => new MailboxAddress(src.Name.Value, src.Address.Value));

    public Task ReceiveAsync(IContext context) => context.Message switch
    {
        SendMail msg => Task.Run(async () =>
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(msg.From.Adapt<MailboxAddress>());
            var to = msg.To.Adapt<IEnumerable<MailboxAddress>>();
            emailMessage.To.AddRange(to);
            emailMessage.Subject = "Test";
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = "Test" };

            using var client = new SmtpClient();
            await client.ConnectAsync("localhost", 1025, SecureSocketOptions.Auto).ConfigureAwait(false);
            await client.SendAsync(emailMessage).ConfigureAwait(false);
            await client.DisconnectAsync(true).ConfigureAwait(false);
        }),
        _ => Task.CompletedTask,
    };

    public record SendMail
    (
       IEnumerable<Email> To,
       Email From,
       IEnumerable<Email> Cc,
       IEnumerable<Email> Bcc
    );

    public record SendMailResult
    (
    );
}
