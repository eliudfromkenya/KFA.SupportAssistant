using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetGroupDTO : BaseDTO<ComputerAssetGroup>
{
  public bool CanBeAssigned { get; set; }
  public string? Description { get; set; }
  public string? GroupName { get; set; }
  public string? LastAssignedValue { get; set; }
  public string? ParentGroupID { get; set; }
  public string? Prefix { get; set; }
  public string? Suffix { get; set; }
  public override ComputerAssetGroup? ToModel()
  {
    return (ComputerAssetGroup)this;
  }

  public static implicit operator ComputerAssetGroupDTO(ComputerAssetGroup obj)
  {
    return new ComputerAssetGroupDTO
    {
      CanBeAssigned = obj.CanBeAssigned,
      Description = obj.Description,
      GroupName = obj.GroupName,
      LastAssignedValue = obj.LastAssignedValue,
      ParentGroupID = obj.ParentGroupID,
      Prefix = obj.Prefix,
      Suffix = obj.Suffix,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetGroup(ComputerAssetGroupDTO obj)
  {
    return new ComputerAssetGroup
    {
      CanBeAssigned = obj.CanBeAssigned,
      Description = obj.Description,
      GroupName = obj.GroupName,
      LastAssignedValue = obj.LastAssignedValue,
      ParentGroupID = obj.ParentGroupID,
      Prefix = obj.Prefix,
      Suffix = obj.Suffix,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
