using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace InvoiceFlow.ArchitectureTests;

public sealed class ArchitectureTests
{
    [Fact]
    public void Domain_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types.InAssembly(typeof(InvoiceFlow.Domain.Common.BaseEntity).Assembly)
            .ShouldNot()
            .HaveDependencyOn("InvoiceFlow.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
