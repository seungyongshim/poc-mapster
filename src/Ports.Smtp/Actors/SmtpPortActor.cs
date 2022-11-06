using FluentEmail.Core;
using MailKit.Net.Smtp;
using MailKit.Security;
using Mapster;
using MimeKit;
using MimeKit.Text;
using Proto;
using Email = WebApplication1.Domains.Email;

namespace Ports.Smtp.Actors;

public class SmtpPortActor : IActor
{
    static SmtpPortActor() => TypeAdapterConfig<Email, MailboxAddress>.NewConfig()
            .MapToConstructor(typeof(MailboxAddress).GetConstructor(new[] { typeof(string), typeof(string) }));

    public Task ReceiveAsync(IContext context) => context.Message switch
    {
        SendMail msg => Task.Run(async () =>
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(msg.From.Name.Value, msg.From.Address.Value));
            emailMessage.To.AddRange(msg.To.Adapt<IEnumerable<MailboxAddress>>());
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
