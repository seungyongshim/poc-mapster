using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domains;
using static WebApplication1.Controllers.SendMailController;
using ext = WebApplication1.Controllers.SendMailControllerExtensions;

namespace WebApplication1.Controllers;

public record SendMailActorRequest
(
    IEnumerable<Email> To,
    Email From,
    IEnumerable<Email> Cc,
    IEnumerable<Email> Bcc
);

public static class SendMailControllerExtensions
{
    public static Email ToEmail(this Dto.EmailContext x) => new(new(x.Name), new(x.EmailAddress));
}

[ApiController]
[Route("[controller]")]
public class SendMailController : ControllerBase
{
    

    [HttpPost]
    public async Task<IActionResult> PostAsync(Dto dto)
    {
        SendMailActorRequest target = new
        (
            To: dto.To.Select(ext.ToEmail),
            From : dto.From.ToEmail(),
            Cc: dto.Cc.Select(ext.ToEmail),
            Bcc: dto.Bcc.Select(ext.ToEmail)
        );

        await Task.CompletedTask;
        return Ok(target);
    }

    

    public record Dto
    {
        public IEnumerable<EmailContext> To { get; init; } = Enumerable.Empty<EmailContext>();
        public EmailContext From { get; init; }
        public IEnumerable<EmailContext> Cc { get; init; } = Enumerable.Empty<EmailContext>();
        public IEnumerable<EmailContext> Bcc { get; init; } = Enumerable.Empty<EmailContext>();

        public readonly record struct EmailContext(string Name, string EmailAddress);
    }
}
