using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ports.Smtp;

public static class Extensions
{
    public static IHostBuilder UseSmtp(this IHostBuilder host)
    {
        host.ConfigureServices((ctx, services) =>
        {
            services.AddFluentEmail("fromemail@test.test")
                    .AddMailKitSender(new FluentEmail.MailKitSmtp.SmtpClientOptions
                    {
                        Server = "localhost",
                        Port = 1025
                    });
        });
        return host;
    }
}
