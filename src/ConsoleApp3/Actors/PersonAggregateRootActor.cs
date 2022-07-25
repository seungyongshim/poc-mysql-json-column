using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ValueObjects;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Proto;
using Proto.Cluster;
using static LanguageExt.Prelude;

namespace ConsoleApp3.Actors;

public class PersonAggregateRootActor : IActor
{
    public PersonAggregateRootActor(IDbContextFactory<AppDbContext> contextFactory)
    {
        ContextFactory = contextFactory;
    }

    public IDbContextFactory<AppDbContext> ContextFactory { get; }
    public Entity<Human> State { get; private set; }

    public Task ReceiveAsync(IContext context)
    {
        var id = Guid.Parse(context.ClusterIdentity()!.Identity);

        return context.Message switch
        {
            Started => Task.Run(async () =>
            {
                await using var conn = await ContextFactory.CreateDbContextAsync();

                var q = retry(Schedule.recurs(5),
                    from __ in unitEff
                    from _1 in conn.Persons.FindAsync(id).ToAff()
                    select _1);

                State = match(await q.Run(), x => x, e => new Entity<Human>()
                {
                    Id = id
                });
            }),

            _ => Task.CompletedTask
        };
    }
}
