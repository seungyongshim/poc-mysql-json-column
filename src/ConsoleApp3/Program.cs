using Boost.Proto.Actor.Hosting.Cluster;
using ConsoleApp3;
using ConsoleApp3.Actors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Proto;
using Proto.Cluster;

var host = Host.CreateDefaultBuilder()
               .UseProtoActorCluster((option, sp) =>
               {
                   option.Provider = ClusterProviderType.Local;
                   option.Name = "poc";

                   option.ClusterKinds.Add(new("Person",
                                           Props.FromProducer(() => new PersonAggregateRootActor(sp.GetRequiredService<IDbContextFactory<AppDbContext>>()))));
               })
               .ConfigureServices(services =>
               {
                   services.AddDbContextFactory<AppDbContext>(options =>
                   {
                       var connectionString = @"Server=127.0.0.1;Database=poc;Uid=root;Pwd=root";
                       var serverVersion = ServerVersion.AutoDetect(connectionString);
                       options.EnableSensitiveDataLogging();
                       options.LogTo(s => Console.WriteLine(s));
                       options.UseMySql(connectionString,
                           serverVersion,
                           options => options.UseMicrosoftJson());
                   });
               }).Build();

await host.StartAsync();

var root = host.Services.GetRequiredService<IRootContext>();
var cluster = host.Services.GetRequiredService<Cluster>();
var cts = new CancellationTokenSource();

var ret = await cluster.RequestAsync<string>($"{Guid.NewGuid()}", "Person", "Hello", cts.Token);

await host.WaitForShutdownAsync();
