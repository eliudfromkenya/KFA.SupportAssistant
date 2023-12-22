namespace KFA.SupportAssistant.Web.EndPoints.CommandDetails;

public class UpdateCommandDetailResponse
{
  public UpdateCommandDetailResponse(CommandDetailRecord commandDetail)
  {
    CommandDetail = commandDetail;
  }

  public CommandDetailRecord CommandDetail { get; set; }
}
