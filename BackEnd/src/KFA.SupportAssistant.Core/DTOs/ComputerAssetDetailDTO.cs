using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetDetailDTO : BaseDTO<ComputerAssetDetail>
{
  public string? AssetName { get; set; }
  public string? Description { get; set; }
  public string? GroupID { get; set; }
  public string? SerialNumber { get; set; }
  public string? State { get; set; }
  public override ComputerAssetDetail? ToModel()
  {
    return (ComputerAssetDetail)this;
  }

  public static implicit operator ComputerAssetDetailDTO(ComputerAssetDetail obj)
  {
    return new ComputerAssetDetailDTO
    {
      AssetName = obj.AssetName,
      Description = obj.Description,
      GroupID = obj.GroupID,
      SerialNumber = obj.SerialNumber,
      State = obj.State,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetDetail(ComputerAssetDetailDTO obj)
  {
    return new ComputerAssetDetail
    {
      AssetName = obj.AssetName,
      Description = obj.Description,
      GroupID = obj.GroupID,
      SerialNumber = obj.SerialNumber,
      State = obj.State,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
