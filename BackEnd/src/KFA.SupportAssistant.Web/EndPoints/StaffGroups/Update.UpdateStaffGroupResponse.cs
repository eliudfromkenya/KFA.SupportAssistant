namespace KFA.SupportAssistant.Web.EndPoints.StaffGroups;

public class UpdateStaffGroupResponse
{
  public UpdateStaffGroupResponse(StaffGroupRecord staffGroup)
  {
    StaffGroup = staffGroup;
  }

  public StaffGroupRecord StaffGroup { get; set; }
}
