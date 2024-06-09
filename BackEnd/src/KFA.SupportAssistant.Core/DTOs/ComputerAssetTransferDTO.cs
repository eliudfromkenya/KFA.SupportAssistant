using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetTransferDTO : BaseDTO<ComputerAssetTransfer>
{
  public string? AssetDetailID { get; set; }
  public string? CostCentreCode { get; set; }
  public string? Narration { get; set; }
  public string? PayrollNumber { get; set; }
  public string? ResponsibleUser { get; set; }
  public string? Status { get; set; }
  public global::System.DateTime TransferDate { get; set; }
  public string? TransferReasons { get; set; }
  public override ComputerAssetTransfer? ToModel()
  {
    return (ComputerAssetTransfer)this;
  }

  public static implicit operator ComputerAssetTransferDTO(ComputerAssetTransfer obj)
  {
    return new ComputerAssetTransferDTO
    {
      AssetDetailID = obj.AssetDetailID,
      CostCentreCode = obj.CostCentreCode,
      Narration = obj.Narration,
      PayrollNumber = obj.PayrollNumber,
      ResponsibleUser = obj.ResponsibleUser,
      Status = obj.Status,
      TransferDate = obj.TransferDate,
      TransferReasons = obj.TransferReasons,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetTransfer(ComputerAssetTransferDTO obj)
  {
    return new ComputerAssetTransfer
    {
      AssetDetailID = obj.AssetDetailID,
      CostCentreCode = obj.CostCentreCode,
      Narration = obj.Narration,
      PayrollNumber = obj.PayrollNumber,
      ResponsibleUser = obj.ResponsibleUser,
      Status = obj.Status,
      TransferDate = obj.TransferDate,
      TransferReasons = obj.TransferReasons,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
