using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetAssignmentDTO : BaseDTO<ComputerAssetAssignment>
{
  public string? AssetID { get; set; }
  public string? AssignedUser { get; set; }
  public global::System.DateTime AssignmentDate { get; set; }
  public string? AssignmentType { get; set; }
  public string? CostCentreCode { get; set; }
  public string? Narration { get; set; }
  public string? PayrollNumber { get; set; }
  public override ComputerAssetAssignment? ToModel()
  {
    return (ComputerAssetAssignment)this;
  }

  public static implicit operator ComputerAssetAssignmentDTO(ComputerAssetAssignment obj)
  {
    return new ComputerAssetAssignmentDTO
    {
      AssetID = obj.AssetID,
      AssignedUser = obj.AssignedUser,
      AssignmentDate = obj.AssignmentDate,
      AssignmentType = obj.AssignmentType,
      CostCentreCode = obj.CostCentreCode,
      Narration = obj.Narration,
      PayrollNumber = obj.PayrollNumber,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetAssignment(ComputerAssetAssignmentDTO obj)
  {
    return new ComputerAssetAssignment
    {
      AssetID = obj.AssetID,
      AssignedUser = obj.AssignedUser,
      AssignmentDate = obj.AssignmentDate,
      AssignmentType = obj.AssignmentType,
      CostCentreCode = obj.CostCentreCode,
      Narration = obj.Narration,
      PayrollNumber = obj.PayrollNumber,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
