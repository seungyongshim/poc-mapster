using Boost.Proto.Actor.DependencyInjection;
using Boost.Proto.Actor.Hosting.Cluster;
using Ports.Smtp.Actors;
using Proto.Router;
using SendMailService;
using SendMailService.Actors;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseProtoActorCluster((option, sp) =>
{
    option.Provider = ClusterProviderType.Local;
    option.Name = "test";

    option.ClusterKinds.Add(new
    (
        nameof(EmailSagaGrain),
        sp.GetRequiredService<IPropsFactory<EmailSagaGrain>>().Create()
    ));

    option.FuncActorSystemStart = root =>
    {
        root.SpawnNamed(root.NewRoundRobinPool(sp.GetRequiredService<IPropsFactory<SmtpPortActor>>().Create(), 10), nameof(SmtpPortActor));

        return root;
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
