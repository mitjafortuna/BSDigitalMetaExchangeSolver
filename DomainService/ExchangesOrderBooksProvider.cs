using System.Text.Json;
using DomainService.Interface;
using DomainService.Model;

namespace DomainService;

public class ExchangesOrderBooksProvider : IExchangesOrderBooksProvider
{
    private readonly string _filePath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Resources",
        "OrderBooks.json"
    );

    public async Task<List<Exchange>> GetExchangesAsync()
    {
        await using var stream = new FileStream(
            _filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: true
        );
        return await JsonSerializer.DeserializeAsync<List<Exchange>>(stream)
            ?? new List<Exchange>();
    }
}
