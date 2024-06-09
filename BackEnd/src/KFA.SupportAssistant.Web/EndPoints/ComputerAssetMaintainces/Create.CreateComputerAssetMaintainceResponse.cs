namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

public readonly struct CreateComputerAssetMaintainceResponse(string? assetDispatchID, string? assetRecieveID, string? assetState, string? description, string? diagnosis, string? doneBy, string? maintainceID, string? narration, bool? treatAsExpense, string? whatWasDone, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AssetDispatchID { get; } = assetDispatchID;
  public string? AssetRecieveID { get; } = assetRecieveID;
  public string? AssetState { get; } = assetState;
  public string? Description { get; } = description;
  public string? Diagnosis { get; } = diagnosis;
  public string? DoneBy { get; } = doneBy;
  public string? MaintainceID { get; } = maintainceID;
  public string? Narration { get; } = narration;
  public bool? TreatAsExpense { get; } = treatAsExpense;
  public string? WhatWasDone { get; } = whatWasDone;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
