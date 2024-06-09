using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

public class CreateComputerAssetTransferRequest
{
  public const string Route = "/computer_asset_transfers";
  public string? AssetDetailID { get; set; }
  public string? CostCentreCode { get; set; }
  public string? Narration { get; set; }
  public string? PayrollNumber { get; set; }
  public string? ResponsibleUser { get; set; }
  public string? Status { get; set; }
  public global::System.DateTime? TransferDate { get; set; }
  [Required]
  public string? TransferID { get; set; }
  public string? TransferReasons { get; set; }
}
