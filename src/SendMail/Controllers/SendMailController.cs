using Microsoft.AspNetCore.Mvc;
using Proto.Cluster;
using WebApplication1.Actors;
using WebApplication1.Domains;
using static WebApplication1.Actors.EmailSagaGrain;

namespace WebApplication1.Controllers;



[ApiController]
[Route("[controller]")]
public class SendMailController : ControllerBase
{
    

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody]Dto dto, [FromServices]Cluster cluster, CancellationToken ct)
    {
        var target = dto.ToSendMail();

        var ret = await cluster.RequestAsync<SendMailResult>("1", nameof(EmailSagaGrain), target, ct);

        return Ok(ret);
    }

    public record Dto
    {
        public IEnumerable<EmailContext> To { get; init; } = Enumerable.Empty<EmailContext>();
        public EmailContext From { get; init; }
        public IEnumerable<EmailContext> Cc { get; init; } = Enumerable.Empty<EmailContext>();
        public IEnumerable<EmailContext> Bcc { get; init; } = Enumerable.Empty<EmailContext>();

        public readonly record struct EmailContext(string Name, string EmailAddress)
        {
            public Email ToEmail() => new(new(Name), new(EmailAddress));
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