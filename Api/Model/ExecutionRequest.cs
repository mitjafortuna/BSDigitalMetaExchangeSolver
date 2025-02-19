using System.ComponentModel.DataAnnotations;

namespace Api.Model;

public class ExecutionRequest
{
    [Required]
    [RegularExpression("^(Buy|Sell)$", ErrorMessage = "OrderType must be either 'Buy' or 'Sell'.")]
    public required string OrderType { get; set; }

    [Required]
    [Range(0.0001, double.MaxValue, ErrorMessage = "OrderAmount must be greater than zero.")]
    public decimal OrderAmount { get; set; }
}
