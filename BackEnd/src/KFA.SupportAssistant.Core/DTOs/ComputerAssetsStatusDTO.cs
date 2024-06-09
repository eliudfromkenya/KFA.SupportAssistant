using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetsStatusDTO : BaseDTO<ComputerAssetsStatus>
{
  public string? AssetDetailID { get; set; }
  public global::System.DateTime Date { get; set; }
  public string? Description { get; set; }
  public string? DoneBy { get; set; }
  public string? Status { get; set; }
  public override ComputerAssetsStatus? ToModel()
  {
    return (ComputerAssetsStatus)this;
  }

  public static implicit operator ComputerAssetsStatusDTO(ComputerAssetsStatus obj)
  {
    return new ComputerAssetsStatusDTO
    {
      AssetDetailID = obj.AssetDetailID,
      Date = obj.Date,
      Description = obj.Description,
      DoneBy = obj.DoneBy,
      Status = obj.Status,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetsStatus(ComputerAssetsStatusDTO obj)
  {
    return new ComputerAssetsStatus
    {
      AssetDetailID = obj.AssetDetailID,
      Date = obj.Date,
      Description = obj.Description,
      DoneBy = obj.DoneBy,
      Status = obj.Status,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
