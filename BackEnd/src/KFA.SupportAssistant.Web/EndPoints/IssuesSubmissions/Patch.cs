using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesSubmissions;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchIssuesSubmissionRequest, IssuesSubmissionRecord>
{
  private const string EndPointId = "ENP-1G6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchIssuesSubmissionRequest.Route));
    //RequestBinder(new PatchBinder<IssuesSubmissionDTO, IssuesSubmission, PatchIssuesSubmissionRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Issues Submission End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a issues submission";
      s.Description = "Used to update part of an existing issues submission. A valid existing issues submission is required.";
      s.ResponseExamples[200] = new IssuesSubmissionRecord("Issue ID", "Narration", Core.Models.Types.IssueStatus.None, "1000", "Submitted To", "Submitting User", DateTime.Now, "Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchIssuesSubmissionRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SubmissionID))
    {
      AddError(request => request.SubmissionID, "The issues submission of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    IssuesSubmissionDTO patchFunc(IssuesSubmissionDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<IssuesSubmissionDTO, IssuesSubmission>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<IssuesSubmissionDTO, IssuesSubmission>(CreateEndPointUser.GetEndPointUser(User), request.SubmissionID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the issues submission to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new IssuesSubmissionRecord(obj.IssueID, obj.Narration, obj.Status, obj.Id, obj.SubmittedTo, obj.SubmittingUser, obj.TimeSubmitted, obj.Type, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
