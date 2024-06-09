using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_computer_asset_maintainces")]
public sealed record class ComputerAssetMaintaince : BaseModel
{
  public override string? ___tableName___ { get; protected set; } = "tbl_computer_asset_maintainces";
  [Column("asset_dispatch_id")]
  public string? AssetDispatchID { get; init; }

  [ForeignKey(nameof(AssetDispatchID))]
  public ComputerAssetsMaintainceDispatch? AssetDispatch { get; set; }
  //[Reactive, NotMapped]
  public string? AssetDispatch_Caption { get; set; }

  [Column("asset_recieve_id")]
  public string? AssetRecieveID { get; init; }

  [ForeignKey(nameof(AssetRecieveID))]
  public ComputerAssetsMaintainceReceiving? AssetRecieve { get; set; }
  //[Reactive, NotMapped]
  public string? AssetRecieve_Caption { get; set; }

  [MaxLength(255, ErrorMessage = "Please asset state must be 255 characters or less")]
  [Column("asset_state")]
  public string? AssetState { get; init; }

  [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
  [Column("description")]
  public string? Description { get; init; }

  [MaxLength(255, ErrorMessage = "Please diagnosis must be 255 characters or less")]
  [Column("diagnosis")]
  public string? Diagnosis { get; init; }

  [MaxLength(255, ErrorMessage = "Please done by must be 255 characters or less")]
  [Column("done_by")]
  public string? DoneBy { get; init; }

  [Required]
  [Column("maintaince_id")]
  public override string? Id { get; init; }

  [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
  [Column("narration")]
  public string? Narration { get; init; }

  [Required]
  [Column("treat_as_expense")]
  public bool TreatAsExpense { get; init; }

  [MaxLength(255, ErrorMessage = "Please what was done must be 255 characters or less")]
  [Column("what_was_done")]
  public string? WhatWasDone { get; init; }

  public ICollection<ComputerAssetsMaintainceAccessory>? ComputerAssetsMaintainceAccessories { get; set; }
  public override object ToBaseDTO()
  {
    return (ComputerAssetMaintainceDTO)this;
  }
}
