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

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

/// <summary>
/// Update an existing project issue.
/// </summary>
/// <remarks>
/// Update an existing project issue by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateProjectIssueRequest, UpdateProjectIssueResponse>
{
  private const string EndPointId = "ENP-1O7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateProjectIssueRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Project Issue End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Project Issue";
      s.Description = "This endpoint is used to update  project issue, making a full replacement of project issue with a specifed valuse. A valid project issue is required.";
      s.ExampleRequest = new UpdateProjectIssueRequest { Category = "Category", Date = DateTime.Now, Description = "Description", Effect = "Effect", Narration = "Narration", ProjectIssueID = "1000", RegisteredBy = "Registered By", Status = 0, SubCategory = "Sub Category", Title = "Title" };
      s.ResponseExamples[200] = new UpdateProjectIssueResponse(new ProjectIssueRecord("Category", DateTime.Now, "Description", "Effect", "Narration", "1000", "Registered By", 0, "Sub Category", "Title", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateProjectIssueRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ProjectIssueID))
    {
      AddError(request => request.ProjectIssueID, "The project issue id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ProjectIssueDTO, ProjectIssue>(CreateEndPointUser.GetEndPointUser(User), request.ProjectIssueID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the project issue to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ProjectIssueDTO, ProjectIssue>(CreateEndPointUser.GetEndPointUser(User), request.ProjectIssueID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateProjectIssueResponse(new ProjectIssueRecord(obj.Category, obj.Date, obj.Description, obj.Effect, obj.Narration, obj.Id, obj.RegisteredBy, obj.Status, obj.SubCategory, obj.Title, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
