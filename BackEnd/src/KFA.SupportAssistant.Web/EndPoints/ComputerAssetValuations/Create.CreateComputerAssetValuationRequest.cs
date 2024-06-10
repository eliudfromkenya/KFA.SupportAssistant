using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetValuations;

public class CreateComputerAssetValuationRequest
{
  public const string Route = "/computer_asset_valuations";

  [Required]
  public string? AssetDetailID { get; set; }

  public string? Description { get; set; }

  [Required]
  public string? RevaluationID { get; set; }

  public string? Status { get; set; }
  public global::System.DateTime? ValuationDate { get; set; }

  [Required]
  public decimal? Value { get; set; }
}
