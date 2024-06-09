using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class VendorDTO : BaseDTO<Vendor>
{
  public string? Contact { get; set; }
  public string? Descriptions { get; set; }
  public string? Email { get; set; }
  public bool IsActive { get; set; }
  public override Vendor? ToModel()
  {
    return (Vendor)this;
  }

  public static implicit operator VendorDTO(Vendor obj)
  {
    return new VendorDTO
    {
      Contact = obj.Contact,
      Descriptions = obj.Descriptions,
      Email = obj.Email,
      IsActive = obj.IsActive,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator Vendor(VendorDTO obj)
  {
    return new Vendor
    {
      Contact = obj.Contact,
      Descriptions = obj.Descriptions,
      Email = obj.Email,
      IsActive = obj.IsActive,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
