namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

public record DeleteSupplierRequest
{
  public const string Route = "/suppliers/{supplierId}";
  public static string BuildRoute(string? supplierId) => Route.Replace("{supplierId}", supplierId);
  public string? SupplierId { get; set; }
}
