namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

public class GetComputerAssetsWriteOffByIdRequest
{
  public const string Route = "/computer_assets_write_offs/{writeOffID}";

  public static string BuildRoute(string? writeOffID) => Route.Replace("{writeOffID}", writeOffID);

  public string? WriteOffID { get; set; }
}
