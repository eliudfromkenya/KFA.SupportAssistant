namespace KFA.SupportAssistant.Web.EndPoints.CountSheetBatches;

public class GetCountSheetBatchByIdRequest
{
  public const string Route = "/count_sheet_batches/{batchKey}";

  public static string BuildRoute(string? batchKey) => Route.Replace("{batchKey}", batchKey);

  public string? BatchKey { get; set; }
}
