using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

public class CreateLeasedPropertiesAccountRequest
{
  public const string Route = "/leased_properties_accounts";
  public decimal? CommencementRent { get; set; }
  public string? CostCentreCode { get; set; }
  public decimal? CurrentRent { get; set; }
  public string? LandlordAddress { get; set; }
  public global::System.DateTime? LastReviewDate { get; set; }
  public global::System.DateTime? LeasedOn { get; set; }

  [Required]
  public string? LeasedPropertyAccountId { get; set; }

  public string? LedgerAccountCode { get; set; }
  public string? Narration { get; set; }
}
