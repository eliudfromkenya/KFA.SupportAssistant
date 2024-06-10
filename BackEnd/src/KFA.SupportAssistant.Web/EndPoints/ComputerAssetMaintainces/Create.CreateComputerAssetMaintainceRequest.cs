using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetMaintainces;

public class CreateComputerAssetMaintainceRequest
{
  public const string Route = "/computer_asset_maintainces";
  public string? AssetDispatchID { get; set; }
  public string? AssetRecieveID { get; set; }
  public string? AssetState { get; set; }
  public string? Description { get; set; }
  public string? Diagnosis { get; set; }
  public string? DoneBy { get; set; }

  [Required]
  public string? MaintainceID { get; set; }

  public string? Narration { get; set; }

  [Required]
  public bool? TreatAsExpense { get; set; }

  public string? WhatWasDone { get; set; }
}
