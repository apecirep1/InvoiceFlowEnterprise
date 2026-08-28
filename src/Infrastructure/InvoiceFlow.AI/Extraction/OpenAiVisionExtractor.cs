using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using InvoiceFlow.Application.Abstractions.AI;

namespace InvoiceFlow.AI.Extraction;

/// <summary>
/// Runnable local fallback extractor. It derives deterministic demo values from the PDF bytes
/// when no external Vision provider is configured.
/// </summary>
public sealed class OpenAiVisionExtractor : IDocumentExtractor
{
    public async Task<ExtractedInvoiceData> ExtractAsync(Stream document, string fileName, CancellationToken cancellationToken)
    {
        using var memory = new MemoryStream();
        await document.CopyToAsync(memory, cancellationToken);
        var bytes = memory.ToArray();

        var hash = Convert.ToHexString(SHA256.HashData(bytes)).Substring(0, 8);
        var amountSeed = BitConverter.ToUInt32(SHA256.HashData(bytes), 0);
        var total = 100m + (amountSeed % 250000) / 100m;

        return new ExtractedInvoiceData(
            InvoiceNumber: $"AI-{hash}",
            VendorName: "Demo Extracted Vendor",
            Total: total,
            Currency: "EUR",
            Confidence: 0.91m);
    }
}
