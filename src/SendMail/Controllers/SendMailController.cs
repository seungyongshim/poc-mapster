using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proto.Cluster;
using SendMailService.Actors;
using WebApplication1.Domains;
using static SendMailService.Actors.EmailSagaGrain;

namespace SendMailService.Controllers;



[ApiController]
[Route("[controller]")]
public class SendMailController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] Dto dto, [FromServices] Cluster cluster, CancellationToken ct)
    {
        var cid = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        var ret = await cluster.RequestAsync<SendMailResult>(ActorPath.EmailSagaGrain(cid), dto.ToSendMail(), ct);

        return Ok(ret);
    }

    public record Dto
    {
        public IEnumerable<EmailContext> To { get; init; } = Enumerable.Empty<EmailContext>();
        public EmailContext From { get; init; }
        public IEnumerable<EmailContext> Cc { get; init; } = Enumerable.Empty<EmailContext>();
        public IEnumerable<EmailContext> Bcc { get; init; } = Enumerable.Empty<EmailContext>();

        public readonly record struct EmailContext(string Name, string Address)
        {
            public Email ToEmail() => new(new(Name), new(Address));
        };

        public SendMail ToSendMail() => new
        (
            To.Select(_ => _.ToEmail()),
            From.ToEmail(),
            Cc.Select(_ => _.ToEmail()),
            Bcc.Select(_ => _.ToEmail())
        );
    }
}
