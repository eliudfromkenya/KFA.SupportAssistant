using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

/// <summary>
/// Get a issues progress by progress id.
/// </summary>
/// <remarks>
/// Takes progress id and returns a matching issues progress record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetIssuesProgressByIdRequest, IssuesProgressRecord>
{
  private const string EndPointId = "ENP-1F4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetIssuesProgressByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Issues Progress End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets issues progress by specified progress id";
      s.Description = "This endpoint is used to retrieve issues progress with the provided progress id";
      s.ExampleRequest = new GetIssuesProgressByIdRequest { ProgressID = "progress id to retrieve" };
      s.ResponseExamples[200] = new IssuesProgressRecord("Description", string.Empty, "Narration", "1000", "Reported By", Core.Models.Types.IssueStatus.None, DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetIssuesProgressByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ProgressID))
    {
      AddError(request => request.ProgressID, "The progress id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<IssuesProgressDTO, IssuesProgress>(CreateEndPointUser.GetEndPointUser(User), request.ProgressID ?? "");
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
      Response = new IssuesProgressRecord(obj.Description, obj.IssueID, obj.Narration, obj.Id, obj.ReportedBy, obj.Status, obj.Time, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
