namespace InvoiceFlow.Infrastructure.Services;

public sealed class AzureBlobStorageService
{
    public async Task<string> SaveLocalFallbackAsync(Stream stream, string fileName, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory("data/uploads");
        var safeName = $"{Guid.NewGuid():N}-{Path.GetFileName(fileName)}";
        var path = Path.Combine("data/uploads", safeName);
        await using var output = File.Create(path);
        await stream.CopyToAsync(output, cancellationToken);
        return path;
    }
}
