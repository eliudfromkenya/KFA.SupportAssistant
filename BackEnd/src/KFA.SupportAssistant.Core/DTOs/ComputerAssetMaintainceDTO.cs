using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class ComputerAssetMaintainceDTO : BaseDTO<ComputerAssetMaintaince>
{
  public string? AssetDispatchID { get; set; }
  public string? AssetRecieveID { get; set; }
  public string? AssetState { get; set; }
  public string? Description { get; set; }
  public string? Diagnosis { get; set; }
  public string? DoneBy { get; set; }
  public string? Narration { get; set; }
  public bool TreatAsExpense { get; set; }
  public string? WhatWasDone { get; set; }
  public override ComputerAssetMaintaince? ToModel()
  {
    return (ComputerAssetMaintaince)this;
  }

  public static implicit operator ComputerAssetMaintainceDTO(ComputerAssetMaintaince obj)
  {
    return new ComputerAssetMaintainceDTO
    {
      AssetDispatchID = obj.AssetDispatchID,
      AssetRecieveID = obj.AssetRecieveID,
      AssetState = obj.AssetState,
      Description = obj.Description,
      Diagnosis = obj.Diagnosis,
      DoneBy = obj.DoneBy,
      Narration = obj.Narration,
      TreatAsExpense = obj.TreatAsExpense,
      WhatWasDone = obj.WhatWasDone,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator ComputerAssetMaintaince(ComputerAssetMaintainceDTO obj)
  {
    return new ComputerAssetMaintaince
    {
      AssetDispatchID = obj.AssetDispatchID,
      AssetRecieveID = obj.AssetRecieveID,
      AssetState = obj.AssetState,
      Description = obj.Description,
      Diagnosis = obj.Diagnosis,
      DoneBy = obj.DoneBy,
      Narration = obj.Narration,
      TreatAsExpense = obj.TreatAsExpense,
      WhatWasDone = obj.WhatWasDone,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
