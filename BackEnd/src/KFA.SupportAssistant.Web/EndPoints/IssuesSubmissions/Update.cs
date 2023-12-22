using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

/// <summary>
/// Update an existing issues submission.
/// </summary>
/// <remarks>
/// Update an existing issues submission by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateIssuesSubmissionRequest, UpdateIssuesSubmissionResponse>
{
  private const string EndPointId = "ENP-1G7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateIssuesSubmissionRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Issues Submission End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Issues Submission";
      s.Description = "This endpoint is used to update  issues submission, making a full replacement of issues submission with a specifed valuse. A valid issues submission is required.";
      s.ExampleRequest = new UpdateIssuesSubmissionRequest { IssueID = "Issue ID", Narration = "Narration", Status = Core.Models.Types.IssueStatus.None, SubmissionID = "1000", SubmittedTo = "Submitted To", SubmittingUser = "Submitting User", TimeSubmitted = "Time Submitted", Type = "Type" };
      s.ResponseExamples[200] = new UpdateIssuesSubmissionResponse(new IssuesSubmissionRecord("Issue ID", "Narration", Core.Models.Types.IssueStatus.None, "1000", "Submitted To", "Submitting User", DateTime.Now, "Type", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateIssuesSubmissionRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SubmissionID))
    {
      AddError(request => request.SubmissionID, "The submission id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<IssuesSubmissionDTO, IssuesSubmission>(CreateEndPointUser.GetEndPointUser(User), request.SubmissionID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the issues submission to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<IssuesSubmissionDTO, IssuesSubmission>(CreateEndPointUser.GetEndPointUser(User), request.SubmissionID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateIssuesSubmissionResponse(new IssuesSubmissionRecord(obj.IssueID, obj.Narration, obj.Status, obj.Id, obj.SubmittedTo, obj.SubmittingUser, obj.TimeSubmitted, obj.Type, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
