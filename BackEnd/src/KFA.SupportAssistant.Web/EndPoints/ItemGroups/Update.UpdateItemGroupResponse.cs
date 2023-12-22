namespace KFA.SupportAssistant.Web.EndPoints.ItemGroups;

public class UpdateItemGroupResponse
{
  public UpdateItemGroupResponse(ItemGroupRecord itemGroup)
  {
    ItemGroup = itemGroup;
  }

  public ItemGroupRecord ItemGroup { get; set; }
}
