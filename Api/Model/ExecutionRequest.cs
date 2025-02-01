using System.ComponentModel.DataAnnotations;

namespace Api.Model;

public class ExecutionRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one exchange must be provided.")]
    public required List<ExchangeRequest> Exchanges { get; set; }

    [Required]
    [RegularExpression("^(Buy|Sell)$", ErrorMessage = "OrderType must be either 'Buy' or 'Sell'.")]
    public required string OrderType { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue, ErrorMessage = "OrderAmount must be greater than zero.")]
    public decimal OrderAmount { get; set; }
}
