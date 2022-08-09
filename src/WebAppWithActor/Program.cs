using System.Data;
using Boost.Proto.Actor.DependencyInjection;
using Boost.Proto.Actor.Hosting.Cluster;
using Domain;
using MySql.Data.MySqlClient;
using WebAppWithActor;
using WebAppWithActor.Actors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(@"Server=127.0.0.1;Database=poc;Uid=root;Pwd=root"));
builder.Services.AddScoped(typeof(GeneralRepository<>));

builder.Host.UseProtoActorCluster((o, sp) =>
{
    o.Name = "poc";
    o.Provider = ClusterProviderType.Local;
    o.ClusterKinds.Add(new("SendSagaActor", sp.GetRequiredService<IPropsFactory<SendSagaActor>>().Create()));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
await app.RunAsync();
