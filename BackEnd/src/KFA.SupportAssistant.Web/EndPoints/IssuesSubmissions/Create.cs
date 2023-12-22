using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

/// <summary>
/// Create a new IssuesSubmission
/// </summary>
/// <remarks>
/// Creates a new issues submission given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateIssuesSubmissionRequest, CreateIssuesSubmissionResponse>
{
  private const string EndPointId = "ENP-1G1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateIssuesSubmissionRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Issues Submission End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new issues submission";
      s.Description = "This endpoint is used to create a new  issues submission. Here details of issues submission to be created is provided";
      s.ExampleRequest = new CreateIssuesSubmissionRequest { IssueID = "Issue ID", Narration = "Narration", Status = Core.Models.Types.IssueStatus.None, SubmissionID = "1000", SubmittedTo = "Submitted To", SubmittingUser = "Submitting User", TimeSubmitted = DateTime.Now, Type = "Type" };
      s.ResponseExamples[200] = new CreateIssuesSubmissionResponse("Issue ID", "Narration",  Core.Models.Types.IssueStatus.None, "1000", "Submitted To", "Submitting User", DateTime.Now, "Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateIssuesSubmissionRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<IssuesSubmissionDTO>();
    requestDTO.Id = request.SubmissionID;

    var result = await mediator.Send(new CreateModelCommand<IssuesSubmissionDTO, IssuesSubmission>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is IssuesSubmissionDTO obj)
      {
        Response = new CreateIssuesSubmissionResponse(obj.IssueID, obj.Narration, obj.Status, obj.Id, obj.SubmittedTo, obj.SubmittingUser, obj.TimeSubmitted, obj.Type, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
