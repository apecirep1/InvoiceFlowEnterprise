using FluentAssertions;
using InvoiceFlow.AI.Extraction;
using Xunit;

namespace InvoiceFlow.AI.IntegrationTests;

public sealed class ExtractionAccuracyTests
{
    [Fact]
    public async Task LocalExtractor_Returns_Deterministic_Data()
    {
        var extractor = new OpenAiVisionExtractor();
        await using var stream = new MemoryStream([1,2,3,4,5]);
        var result = await extractor.ExtractAsync(stream, "sample.pdf", CancellationToken.None);
        result.InvoiceNumber.Should().StartWith("AI-");
        result.Total.Should().BeGreaterThan(0);
        result.Confidence.Should().BeGreaterThan(0.8m);
    }
}
