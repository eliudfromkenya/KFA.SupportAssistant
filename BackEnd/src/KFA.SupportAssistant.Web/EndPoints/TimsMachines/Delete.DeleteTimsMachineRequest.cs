namespace KFA.SupportAssistant.Web.EndPoints.TimsMachines;

public record DeleteTimsMachineRequest
{
  public const string Route = "/tims_machines/{machineID}";
  public static string BuildRoute(string? machineID) => Route.Replace("{machineID}", machineID);
  public string? MachineID { get; set; }
}
