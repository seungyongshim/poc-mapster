using Proto;
using WebApplication1.Domains;

namespace Ports.Smtp.Actors;

public class SmtpPortActor : IActor
{
    public Task ReceiveAsync(IContext context) => throw new NotImplementedException();

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
