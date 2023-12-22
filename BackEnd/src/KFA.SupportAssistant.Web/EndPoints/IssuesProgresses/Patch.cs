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

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchIssuesProgressRequest, IssuesProgressRecord>
{
  private const string EndPointId = "ENP-1F6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchIssuesProgressRequest.Route));
    //RequestBinder(new PatchBinder<IssuesProgressDTO, IssuesProgress, PatchIssuesProgressRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Issues Progress End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a issues progress";
      s.Description = "Used to update part of an existing issues progress. A valid existing issues progress is required.";
      s.ResponseExamples[200] = new IssuesProgressRecord("Description", string.Empty, "Narration", "1000", "Reported By", Core.Models.Types.IssueStatus.None, DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchIssuesProgressRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ProgressID))
    {
      AddError(request => request.ProgressID, "The issues progress of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    IssuesProgressDTO patchFunc(IssuesProgressDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<IssuesProgressDTO, IssuesProgress>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<IssuesProgressDTO, IssuesProgress>(CreateEndPointUser.GetEndPointUser(User), request.ProgressID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the issues progress to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new IssuesProgressRecord(obj.Description, obj.IssueID, obj.Narration, obj.Id, obj.ReportedBy, obj.Status, obj.Time, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
