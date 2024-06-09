using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetsMaintainceReceivingDTO : BaseDTO<ComputerAssetsMaintainceReceiving>
{
  public string? AssetDetailID { get; set; }
  public string? BroughtBy { get; set; }
  public global::System.DateTime DateRecieved { get; set; }
  public string? Description { get; set; }
  public string? FromDepartmentCode { get; set; }
  public string? Narration { get; set; }
  public string? ReasonToSend { get; set; }
  public string? RecievedBy { get; set; }
  public override ComputerAssetsMaintainceReceiving? ToModel()
  {
    return (ComputerAssetsMaintainceReceiving)this;
  }

  public static implicit operator ComputerAssetsMaintainceReceivingDTO(ComputerAssetsMaintainceReceiving obj)
  {
    return new ComputerAssetsMaintainceReceivingDTO
    {
      AssetDetailID = obj.AssetDetailID,
      BroughtBy = obj.BroughtBy,
      DateRecieved = obj.DateRecieved,
      Description = obj.Description,
      FromDepartmentCode = obj.FromDepartmentCode,
      Narration = obj.Narration,
      ReasonToSend = obj.ReasonToSend,
      RecievedBy = obj.RecievedBy,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetsMaintainceReceiving(ComputerAssetsMaintainceReceivingDTO obj)
  {
    return new ComputerAssetsMaintainceReceiving
    {
      AssetDetailID = obj.AssetDetailID,
      BroughtBy = obj.BroughtBy,
      DateRecieved = obj.DateRecieved,
      Description = obj.Description,
      FromDepartmentCode = obj.FromDepartmentCode,
      Narration = obj.Narration,
      ReasonToSend = obj.ReasonToSend,
      RecievedBy = obj.RecievedBy,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
