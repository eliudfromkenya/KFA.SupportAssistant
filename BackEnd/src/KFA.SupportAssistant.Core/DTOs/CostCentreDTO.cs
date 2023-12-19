using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class CostCentreDTO : BaseDTO<CostCentre>
{
  public string? Description { get; set; }
  public string? Narration { get; set; }
  public bool? IsActive { get; set; } = true;
  public string? Region { get; set; }
  public string? SupplierCodePrefix { get; set; }
  public override CostCentre? ToModel()
  {
    return (CostCentre)this;
  }

  public static implicit operator CostCentreDTO(CostCentre obj)
  {
    return new CostCentreDTO
    {
      Description = obj.Description,
      Narration = obj.Narration,
      Region = obj.Region,
      IsActive = obj.IsActive,
      SupplierCodePrefix = obj.SupplierCodePrefix,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator CostCentre(CostCentreDTO obj)
  {
    return new CostCentre
    {
      Description = obj.Description,
      Narration = obj.Narration,
      IsActive = obj.IsActive,
      Region = obj.Region,
      SupplierCodePrefix = obj.SupplierCodePrefix,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
