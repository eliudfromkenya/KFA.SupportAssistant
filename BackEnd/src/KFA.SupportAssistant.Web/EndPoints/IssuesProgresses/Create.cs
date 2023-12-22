using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

/// <summary>
/// Create a new IssuesProgress
/// </summary>
/// <remarks>
/// Creates a new issues progress given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateIssuesProgressRequest, CreateIssuesProgressResponse>
{
  private const string EndPointId = "ENP-1F1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateIssuesProgressRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Issues Progress End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new issues progress";
      s.Description = "This endpoint is used to create a new  issues progress. Here details of issues progress to be created is provided";
      s.ExampleRequest = new CreateIssuesProgressRequest { Description = "Description", IssueID = string.Empty, Narration = "Narration", ProgressID = "1000", ReportedBy = "Reported By", Status = Core.Models.Types.IssueStatus.None, Time = DateTime.Now };
      s.ResponseExamples[200] = new CreateIssuesProgressResponse("Description", string.Empty, "Narration", "1000", "Reported By",  Core.Models.Types.IssueStatus.None, DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateIssuesProgressRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<IssuesProgressDTO>();
    requestDTO.Id = request.ProgressID;

    var result = await mediator.Send(new CreateModelCommand<IssuesProgressDTO, IssuesProgress>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is IssuesProgressDTO obj)
      {
        Response = new CreateIssuesProgressResponse(obj.Description, obj.IssueID, obj.Narration, obj.Id, obj.ReportedBy, obj.Status, obj.Time, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
