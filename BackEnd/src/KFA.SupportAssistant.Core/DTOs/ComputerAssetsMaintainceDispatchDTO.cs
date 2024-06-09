using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetsMaintainceDispatchDTO : BaseDTO<ComputerAssetsMaintainceDispatch>
{
  public string? AssetDetailID { get; set; }
  public string? CollectedBy { get; set; }
  public global::System.DateTime DateSend { get; set; }
  public string? Description { get; set; }
  public string? Narration { get; set; }
  public string? ReasonToSend { get; set; }
  public string? SentBy { get; set; }
  public string? ToDepartmentCode { get; set; }
  public override ComputerAssetsMaintainceDispatch? ToModel()
  {
    return (ComputerAssetsMaintainceDispatch)this;
  }

  public static implicit operator ComputerAssetsMaintainceDispatchDTO(ComputerAssetsMaintainceDispatch obj)
  {
    return new ComputerAssetsMaintainceDispatchDTO
    {
      AssetDetailID = obj.AssetDetailID,
      CollectedBy = obj.CollectedBy,
      DateSend = obj.DateSend,
      Description = obj.Description,
      Narration = obj.Narration,
      ReasonToSend = obj.ReasonToSend,
      SentBy = obj.SentBy,
      ToDepartmentCode = obj.ToDepartmentCode,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetsMaintainceDispatch(ComputerAssetsMaintainceDispatchDTO obj)
  {
    return new ComputerAssetsMaintainceDispatch
    {
      AssetDetailID = obj.AssetDetailID,
      CollectedBy = obj.CollectedBy,
      DateSend = obj.DateSend,
      Description = obj.Description,
      Narration = obj.Narration,
      ReasonToSend = obj.ReasonToSend,
      SentBy = obj.SentBy,
      ToDepartmentCode = obj.ToDepartmentCode,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
