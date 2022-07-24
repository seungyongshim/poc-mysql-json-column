using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp3.Entities;
using ConsoleApp3.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Proto;
using Proto.Cluster;

namespace ConsoleApp3.Actors;

public class PersonAggregateRootActor : IActor
{
    public PersonAggregateRootActor(IDbContextFactory<AppDbContext> contextFactory)
    {
        ContextFactory = contextFactory;
    }

    public IDbContextFactory<AppDbContext> ContextFactory { get; }
    public Entity<Person> State { get; private set; }

    public Task ReceiveAsync(IContext context)
    {
        var id = Guid.Parse(context.ClusterIdentity()!.Identity);

        return context.Message switch
        {

            Started => Task.Run(async () =>
            {
                await using var conn = await ContextFactory.CreateDbContextAsync();

                State = await conn.Persons.FindAsync(id) ?? new Entity<Person>()
                {
                    Id = id
                };
            }),

            _ => Task.CompletedTask
        };
    }
}
