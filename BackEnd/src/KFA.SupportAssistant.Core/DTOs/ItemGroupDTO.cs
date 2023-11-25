using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ItemGroupDTO : BaseDTO<ItemGroup>
{
  public string? Name { get; set; }
  public string? ParentGroupId { get; set; }
  public override ItemGroup? ToModel()
  {
    return (ItemGroup)this;
  }
  public static implicit operator ItemGroupDTO(ItemGroup obj)
  {
    return new ItemGroupDTO
    {
      Name = obj.Name,
      ParentGroupId = obj.ParentGroupId,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ItemGroup(ItemGroupDTO obj)
  {
    return new ItemGroup
    {
      Name = obj.Name,
      ParentGroupId = obj.ParentGroupId,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
