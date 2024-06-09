using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetsPostsToDynamicDTO : BaseDTO<ComputerAssetsPostsToDynamic>
{
  public string? AssetDetailID { get; set; }
  public global::System.DateTime DateGenerated { get; set; }
  public string? Description { get; set; }
  public bool Posted { get; set; }
  public override ComputerAssetsPostsToDynamic? ToModel()
  {
    return (ComputerAssetsPostsToDynamic)this;
  }

  public static implicit operator ComputerAssetsPostsToDynamicDTO(ComputerAssetsPostsToDynamic obj)
  {
    return new ComputerAssetsPostsToDynamicDTO
    {
      AssetDetailID = obj.AssetDetailID,
      DateGenerated = obj.DateGenerated,
      Description = obj.Description,
      Posted = obj.Posted,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetsPostsToDynamic(ComputerAssetsPostsToDynamicDTO obj)
  {
    return new ComputerAssetsPostsToDynamic
    {
      AssetDetailID = obj.AssetDetailID,
      DateGenerated = obj.DateGenerated,
      Description = obj.Description,
      Posted = obj.Posted,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
