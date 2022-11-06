using Mapster;
using MimeKit;
using SendMailService.Domain;

namespace Ports.Smtp.Tests;

public class UnitTest1
{
    static UnitTest1()
    {
        var ctor = typeof(MailboxAddress).GetConstructor(new[] { typeof(string), typeof(string) });
        TypeAdapterConfig<Email, MailboxAddress>.NewConfig()
            .ConstructUsing(src => new MailboxAddress(src.Name.Value, src.Address.Value))
            .MapToConstructor(ctor);
    }

    [Fact]
    public void Test1()
    {
        
        var sut = new Email(new("Hong"), new("hong@hong.com"));
        var ret = sut.Adapt<MailboxAddress>();
    }

    [Fact]
    public void Test2()
    {
        var sut = new Email(new("Hong"), new("hong@hong.com"));
        var ret = new MailboxAddress(sut.Name.Value, sut.Address.Value);
    }
}
