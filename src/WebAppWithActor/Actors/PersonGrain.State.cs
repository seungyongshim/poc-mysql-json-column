using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;
using ConsoleAppWithoutEfCore;
using Domain.Entities;
using Json.More;
using Newtonsoft.Json.Converters;
using Proto.DependencyInjection;

namespace WebAppWithActor.Actors;

public enum PersonGrainFsm
{
    NotReady,
    Ready,
    Send,
    Sending,
    Sent,
    Report
}

public record PersonActorState
{
    public string Name { get; init; }
    public string Description { get; init; }
    public PersonGrainFsm Fsm { get; init; } 
    public int Count { get; init; }
};
