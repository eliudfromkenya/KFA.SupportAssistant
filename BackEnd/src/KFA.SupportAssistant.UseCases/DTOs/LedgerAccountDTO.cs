using KFA.DynamicsAssistant.Infrastructure.Models;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class LedgerAccountDTO : BaseDTO<LedgerAccount>
{
  public string? CostCentreCode { get; set; }
  public string? Description { get; set; }
  public string? GroupName { get; set; }
  public bool IncreaseWithDebit { get; set; }
  public string? LedgerAccountCode { get; set; }
  public string? MainGroup { get; set; }
  public string? Narration { get; set; }
  public override LedgerAccount? ToModel()
  {
    return (LedgerAccount)this;
  }
  public static implicit operator LedgerAccountDTO(LedgerAccount obj)
  {
    return new LedgerAccountDTO
    {
      CostCentreCode = obj.CostCentreCode,
      Description = obj.Description,
      GroupName = obj.GroupName,
      IncreaseWithDebit = obj.IncreaseWithDebit,
      LedgerAccountCode = obj.LedgerAccountCode,
      MainGroup = obj.MainGroup,
      Narration = obj.Narration,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator LedgerAccount(LedgerAccountDTO obj)
  {
    return new LedgerAccount
    {
      CostCentreCode = obj.CostCentreCode,
      Description = obj.Description,
      GroupName = obj.GroupName,
      IncreaseWithDebit = obj.IncreaseWithDebit,
      LedgerAccountCode = obj.LedgerAccountCode,
      MainGroup = obj.MainGroup,
      Narration = obj.Narration,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
