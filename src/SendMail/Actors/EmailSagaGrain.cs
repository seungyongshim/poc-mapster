using Proto;
using WebApplication1.Domains;

namespace WebApplication1.Actors;

public class EmailSagaGrain : IActor
{
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

    public Task ReceiveAsync(IContext context)
    {
        

        return Task.CompletedTask;
    }
}
