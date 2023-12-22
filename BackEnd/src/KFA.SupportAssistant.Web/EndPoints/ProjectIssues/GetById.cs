using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

/// <summary>
/// Get a project issue by project issue id.
/// </summary>
/// <remarks>
/// Takes project issue id and returns a matching project issue record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetProjectIssueByIdRequest, ProjectIssueRecord>
{
  private const string EndPointId = "ENP-1O4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetProjectIssueByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Project Issue End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets project issue by specified project issue id";
      s.Description = "This endpoint is used to retrieve project issue with the provided project issue id";
      s.ExampleRequest = new GetProjectIssueByIdRequest { ProjectIssueID = "project issue id to retrieve" };
      s.ResponseExamples[200] = new ProjectIssueRecord("Category", DateTime.Now, "Description", "Effect", "Narration", "1000", "Registered By", 0, "Sub Category", "Title", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetProjectIssueByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ProjectIssueID))
    {
      AddError(request => request.ProjectIssueID, "The project issue id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ProjectIssueDTO, ProjectIssue>(CreateEndPointUser.GetEndPointUser(User), request.ProjectIssueID ?? "");
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
      Response = new ProjectIssueRecord(obj.Category, obj.Date, obj.Description, obj.Effect, obj.Narration, obj.Id, obj.RegisteredBy, obj.Status, obj.SubCategory, obj.Title, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
