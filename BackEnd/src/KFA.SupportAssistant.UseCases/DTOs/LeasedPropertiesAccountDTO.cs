using KFA.SupportAssistant.Infrastructure.Models;
using KFA.SupportAssistant;

namespace KFA.SupportAssistant.UseCases.DTOs;
public record class LeasedPropertiesAccountDTO : BaseDTO<LeasedPropertiesAccount>
{
  public string? AccountNumber { get; set; }
  public decimal CommencementRent { get; set; }
  public string? CostCentreCode { get; set; }
  public decimal CurrentRent { get; set; }
  public string? LandlordAddress { get; set; }
  public DateTime LastReviewDate { get; set; }
  public DateTime LeasedOn { get; set; }
  public string? LedgerAccountId { get; set; }
  public string? Narration { get; set; }
  public override LeasedPropertiesAccount? ToModel()
  {
    return (LeasedPropertiesAccount)this;
  }
  public static implicit operator LeasedPropertiesAccountDTO(LeasedPropertiesAccount obj)
  {
    return new LeasedPropertiesAccountDTO
    {
      AccountNumber = obj.AccountNumber,
      CommencementRent = obj.CommencementRent,
      CostCentreCode = obj.CostCentreCode,
      CurrentRent = obj.CurrentRent,
      LandlordAddress = obj.LandlordAddress,
      LastReviewDate = obj.LastReviewDate,
      LeasedOn = obj.LeasedOn,
      LedgerAccountId = obj.LedgerAccountId,
      Narration = obj.Narration,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator LeasedPropertiesAccount(LeasedPropertiesAccountDTO obj)
  {
    return new LeasedPropertiesAccount
    {
      AccountNumber = obj.AccountNumber,
      CommencementRent = obj.CommencementRent,
      CostCentreCode = obj.CostCentreCode,
      CurrentRent = obj.CurrentRent,
      LandlordAddress = obj.LandlordAddress,
      LastReviewDate = obj.LastReviewDate,
      LeasedOn = obj.LeasedOn,
      LedgerAccountId = obj.LedgerAccountId,
      Narration = obj.Narration,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
