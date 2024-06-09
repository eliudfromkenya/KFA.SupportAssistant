using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetValuationDTO : BaseDTO<ComputerAssetValuation>
{
  public string? AssetDetailID { get; set; }
  public string? Description { get; set; }
  public string? Status { get; set; }
  public global::System.DateTime ValuationDate { get; set; }
  public decimal Value { get; set; }
  public override ComputerAssetValuation? ToModel()
  {
    return (ComputerAssetValuation)this;
  }

  public static implicit operator ComputerAssetValuationDTO(ComputerAssetValuation obj)
  {
    return new ComputerAssetValuationDTO
    {
      AssetDetailID = obj.AssetDetailID,
      Description = obj.Description,
      Status = obj.Status,
      ValuationDate = obj.ValuationDate,
      Value = obj.Value,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetValuation(ComputerAssetValuationDTO obj)
  {
    return new ComputerAssetValuation
    {
      AssetDetailID = obj.AssetDetailID,
      Description = obj.Description,
      Status = obj.Status,
      ValuationDate = obj.ValuationDate,
      Value = obj.Value,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
