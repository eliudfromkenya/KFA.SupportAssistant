using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Core.Models.Types;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

/// <summary>
/// Update an existing issues progress.
/// </summary>
/// <remarks>
/// Update an existing issues progress by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateIssuesProgressRequest, UpdateIssuesProgressResponse>
{
  private const string EndPointId = "ENP-1F7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateIssuesProgressRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Issues Progress End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Issues Progress";
      s.Description = "This endpoint is used to update  issues progress, making a full replacement of issues progress with a specifed valuse. A valid issues progress is required.";
      s.ExampleRequest = new UpdateIssuesProgressRequest { Description = "Description", IssueID = string.Empty, Narration = "Narration", ProgressID = "1000", ReportedBy = "Reported By", Status = "Status", Time = DateTime.Now };
      s.ResponseExamples[200] = new UpdateIssuesProgressResponse(new IssuesProgressRecord("Description", string.Empty, "Narration", "1000", "Reported By", Core.Models.Types.IssueStatus.None, DateTime.Now, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateIssuesProgressRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ProgressID))
    {
      AddError(request => request.ProgressID, "The progress id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<IssuesProgressDTO, IssuesProgress>(CreateEndPointUser.GetEndPointUser(User), request.ProgressID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the issues progress to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<IssuesProgressDTO, IssuesProgress>(CreateEndPointUser.GetEndPointUser(User), request.ProgressID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateIssuesProgressResponse(new IssuesProgressRecord(obj.Description, obj.IssueID, obj.Narration, obj.Id, obj.ReportedBy, obj.Status, obj.Time, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
