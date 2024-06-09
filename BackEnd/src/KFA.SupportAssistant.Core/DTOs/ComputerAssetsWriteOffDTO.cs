using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetsWriteOffDTO : BaseDTO<ComputerAssetsWriteOff>
{
  public string? AssetDetailID { get; set; }
  public string? Description { get; set; }
  public string? Narration { get; set; }
  public string? ReasonForWriteOff { get; set; }
  public global::System.DateTime WriteOffDate { get; set; }
  public override ComputerAssetsWriteOff? ToModel()
  {
    return (ComputerAssetsWriteOff)this;
  }

  public static implicit operator ComputerAssetsWriteOffDTO(ComputerAssetsWriteOff obj)
  {
    return new ComputerAssetsWriteOffDTO
    {
      AssetDetailID = obj.AssetDetailID,
      Description = obj.Description,
      Narration = obj.Narration,
      ReasonForWriteOff = obj.ReasonForWriteOff,
      WriteOffDate = obj.WriteOffDate,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetsWriteOff(ComputerAssetsWriteOffDTO obj)
  {
    return new ComputerAssetsWriteOff
    {
      AssetDetailID = obj.AssetDetailID,
      Description = obj.Description,
      Narration = obj.Narration,
      ReasonForWriteOff = obj.ReasonForWriteOff,
      WriteOffDate = obj.WriteOffDate,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
