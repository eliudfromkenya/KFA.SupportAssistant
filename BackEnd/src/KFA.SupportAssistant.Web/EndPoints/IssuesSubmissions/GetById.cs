using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

/// <summary>
/// Get a issues submission by submission id.
/// </summary>
/// <remarks>
/// Takes submission id and returns a matching issues submission record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetIssuesSubmissionByIdRequest, IssuesSubmissionRecord>
{
  private const string EndPointId = "ENP-1G4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetIssuesSubmissionByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Issues Submission End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets issues submission by specified submission id";
      s.Description = "This endpoint is used to retrieve issues submission with the provided submission id";
      s.ExampleRequest = new GetIssuesSubmissionByIdRequest { SubmissionID = "submission id to retrieve" };
      s.ResponseExamples[200] = new IssuesSubmissionRecord("Issue ID", "Narration", Core.Models.Types.IssueStatus.None, "1000", "Submitted To", "Submitting User", DateTime.Now, "Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetIssuesSubmissionByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SubmissionID))
    {
      AddError(request => request.SubmissionID, "The submission id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<IssuesSubmissionDTO, IssuesSubmission>(CreateEndPointUser.GetEndPointUser(User), request.SubmissionID ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound || result.Value == null)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }
    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new IssuesSubmissionRecord(obj.IssueID, obj.Narration, obj.Status, obj.Id, obj.SubmittedTo, obj.SubmittingUser, obj.TimeSubmitted, obj.Type, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
