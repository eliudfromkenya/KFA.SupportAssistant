using KFA.DynamicsAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class LetPropertiesAccountDTO : BaseDTO<LetPropertiesAccount>
{
  public string? AccountNumber { get; set; }
  public decimal CommencementRent { get; set; }
  public string? CostCentreCode { get; set; }
  public decimal CurrentRent { get; set; }
  public DateTime LastReviewDate { get; set; }
  public string? LedgerAccountId { get; set; }
  public DateTime LetOn { get; set; }
  public string? Narration { get; set; }
  public string? TenantAddress { get; set; }
  public override LetPropertiesAccount? ToModel()
  {
    return (LetPropertiesAccount)this;
  }
  public static implicit operator LetPropertiesAccountDTO(LetPropertiesAccount obj)
  {
    return new LetPropertiesAccountDTO
    {
      AccountNumber = obj.AccountNumber,
      CommencementRent = obj.CommencementRent,
      CostCentreCode = obj.CostCentreCode,
      CurrentRent = obj.CurrentRent,
      LastReviewDate = obj.LastReviewDate,
      LedgerAccountId = obj.LedgerAccountId,
      LetOn = obj.LetOn,
      Narration = obj.Narration,
      TenantAddress = obj.TenantAddress,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator LetPropertiesAccount(LetPropertiesAccountDTO obj)
  {
    return new LetPropertiesAccount
    {
      AccountNumber = obj.AccountNumber,
      CommencementRent = obj.CommencementRent,
      CostCentreCode = obj.CostCentreCode,
      CurrentRent = obj.CurrentRent,
      LastReviewDate = obj.LastReviewDate,
      LedgerAccountId = obj.LedgerAccountId,
      LetOn = obj.LetOn,
      Narration = obj.Narration,
      TenantAddress = obj.TenantAddress,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
