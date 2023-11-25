using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class SupplierDTO : BaseDTO<Supplier>
{
  public string? Address { get; set; }
  public string? ContactPerson { get; set; }
  public string? CostCentreCode { get; set; }
  public string? Description { get; set; }
  public string? Email { get; set; }
  public string? Narration { get; set; }
  public string? PostalCode { get; set; }
  public string? SupplierCode { get; set; }
  public string? SupplierLedgerAccountId { get; set; }
  public string? Telephone { get; set; }
  public string? Town { get; set; }
  public override Supplier? ToModel()
  {
    return (Supplier)this;
  }
  public static implicit operator SupplierDTO(Supplier obj)
  {
    return new SupplierDTO
    {
      Address = obj.Address,
      ContactPerson = obj.ContactPerson,
      CostCentreCode = obj.CostCentreCode,
      Description = obj.Description,
      Email = obj.Email,
      Narration = obj.Narration,
      PostalCode = obj.PostalCode,
      SupplierCode = obj.SupplierCode,
      SupplierLedgerAccountId = obj.SupplierLedgerAccountId,
      Telephone = obj.Telephone,
      Town = obj.Town,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator Supplier(SupplierDTO obj)
  {
    return new Supplier
    {
      Address = obj.Address,
      ContactPerson = obj.ContactPerson,
      CostCentreCode = obj.CostCentreCode,
      Description = obj.Description,
      Email = obj.Email,
      Narration = obj.Narration,
      PostalCode = obj.PostalCode,
      SupplierCode = obj.SupplierCode,
      SupplierLedgerAccountId = obj.SupplierLedgerAccountId,
      Telephone = obj.Telephone,
      Town = obj.Town,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
