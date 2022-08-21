using System.Data;
using Boost.Proto.Actor.DependencyInjection;
using Boost.Proto.Actor.Hosting.Cluster;
using ConsoleAppWithoutEfCore;
using Domain;
using MySql.Data.MySqlClient;
using WebAppWithActor;
using WebAppWithActor.Actors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(@"Server=127.0.0.1;Database=poc;Uid=root;Pwd=root"));

builder.Host.UseProtoActorCluster((o, sp) =>
{
    o.FuncClusterConfig = config =>
    {
        return config;
    };
    o.Name = "poc";
    o.Provider = ClusterProviderType.Local;
    o.ClusterKinds.Add(new(nameof(SendSagaActor), sp.GetRequiredService<IPropsFactory<SendSagaActor>>().Create()));
    o.ClusterKinds.Add(new(nameof(RepositoryActor), sp.GetRequiredService<IPropsFactory<RepositoryActor>>().Create()));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
await app.RunAsync();
