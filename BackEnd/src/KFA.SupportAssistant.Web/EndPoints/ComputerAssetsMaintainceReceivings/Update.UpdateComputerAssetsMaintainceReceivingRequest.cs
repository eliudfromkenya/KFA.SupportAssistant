using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

public record UpdateComputerAssetsMaintainceReceivingRequest
{
  public const string Route = "/computer_assets_maintaince_receivings/{receiveID}";
  public string? AssetDetailID { get; set; }
  public string? BroughtBy { get; set; }
  public global::System.DateTime? DateRecieved { get; set; }
  public string? Description { get; set; }
  public string? FromDepartmentCode { get; set; }
  public string? Narration { get; set; }
  public string? ReasonToSend { get; set; }
  [Required]
  public string? ReceiveID { get; set; }
  public string? RecievedBy { get; set; }
}
