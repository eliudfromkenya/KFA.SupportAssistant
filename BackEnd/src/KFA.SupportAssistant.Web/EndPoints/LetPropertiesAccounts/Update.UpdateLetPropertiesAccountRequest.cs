using System.ComponentModel.DataAnnotations;

namespace KFA.SupportAssistant.Web.EndPoints.LetPropertiesAccounts;

public record UpdateLetPropertiesAccountRequest
{
  public const string Route = "/let_properties_accounts/{letPropertyAccountId}";
  public decimal? CommencementRent { get; set; }
  public string? CostCentreCode { get; set; }
  public decimal? CurrentRent { get; set; }
  public global::System.DateTime? LastReviewDate { get; set; }
  public string? LedgerAccountCode { get; set; }
  public global::System.DateTime? LetOn { get; set; }
  [Required]
  public string? LetPropertyAccountId { get; set; }
  public string? Narration { get; set; }
  public string? TenantAddress { get; set; }
}
