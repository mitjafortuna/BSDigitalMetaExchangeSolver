using DomainService.Model;

namespace DomainService.Interface
{
    /// <summary>
    /// Interface for ExchangesOrderBooksProvider
    /// </summary>
    public interface IExchangesOrderBooksProvider
    {
        /// <summary>
        /// Get exchanges with order books loaded form json file
        /// </summary>
        /// <returns></returns>
        Task<List<Exchange>> GetExchangesAsync();
    }
}
