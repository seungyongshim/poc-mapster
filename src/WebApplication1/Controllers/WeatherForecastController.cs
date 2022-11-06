using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domains;

namespace WebApplication1.Controllers;

public record RequestSendMailActor
(
    IEnumerable<Email> To,
    Email From,
    IEnumerable<Email> Cc,
    IEnumerable<Email> Bcc
);

[ApiController]
[Route("[controller]")]
public class SendMailController : ControllerBase
{
    

    [HttpPost]
    public async Task<IActionResult> PostAsync(Dto dto)
    {
        var target = dto.ToRequestSendMailActor();
        await Task.CompletedTask;
        return Ok(new
        {
            To = target.To.Select(x => new
            {
                Name = x.Name.Value,
                Address = x.EmailAddress.Value
            })
        });
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

        public RequestSendMailActor ToRequestSendMailActor() => new
        (
            To.Select(_ => _.ToEmail()),
            From.ToEmail(),
            Cc.Select(_ => _.ToEmail()),
            Bcc.Select(_ => _.ToEmail())
        );
    }
}
