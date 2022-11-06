using Boost.Proto.Actor.DependencyInjection;
using Boost.Proto.Actor.Hosting.Cluster;
using WebApplication1.Actors;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseProtoActorCluster((op, sp) =>
{
    op.Provider = ClusterProviderType.Local;
    op.Name = "test";

    op.ClusterKinds.Add(new
    (
        "EmailSagaGrain",
        sp.GetRequiredService<IPropsFactory<EmailSagaGrain>>().Create()
    ));
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
