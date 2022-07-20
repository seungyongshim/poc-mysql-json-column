using System;
using Xunit;
using static Sample.Prelude;

namespace Sample.Tests;

public class PreludeSpec
{
    [Fact]
    public void AddSuccess()
    {
        Assert.Equal(3, add(1)(2));
    }
}
