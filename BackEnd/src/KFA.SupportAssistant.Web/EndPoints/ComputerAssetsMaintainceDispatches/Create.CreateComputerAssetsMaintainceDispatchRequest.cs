using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

public class CreateComputerAssetsMaintainceDispatchRequest
{
  public const string Route = "/computer_assets_maintaince_dispatches";
  public string? AssetDetailID { get; set; }
  public string? CollectedBy { get; set; }
  public global::System.DateTime? DateSend { get; set; }
  public string? Description { get; set; }

  [Required]
  public string? DispatchID { get; set; }

  public string? Narration { get; set; }
  public string? ReasonToSend { get; set; }
  public string? SentBy { get; set; }
  public string? ToDepartmentCode { get; set; }
}
