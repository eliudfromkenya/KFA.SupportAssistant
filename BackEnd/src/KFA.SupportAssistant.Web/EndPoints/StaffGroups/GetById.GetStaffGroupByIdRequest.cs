namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

public class GetStaffGroupByIdRequest
{
  public const string Route = "/staff_groups/{groupNumber}";

  public static string BuildRoute(string? groupNumber) => Route.Replace("{groupNumber}", groupNumber);

  public string? GroupNumber { get; set; }
}
