using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

/// <summary>
/// List all project issues by specified conditions
/// </summary>
/// <remarks>
/// List all project issues - returns a ProjectIssueListResponse containing the project issues.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, ProjectIssueListResponse>
{
  private const string EndPointId = "ENP-1O5";
  public const string Route = "/project_issues";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Project Issues List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of project issues as specified";
      s.Description = "Returns all project issues as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new ProjectIssueListResponse { ProjectIssues = [new ProjectIssueRecord("Category", DateTime.Now, "Description", "Effect", "Narration", "1000", "Registered By", 0, "Sub Category", "Title", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<ProjectIssueDTO, ProjectIssue>(CreateEndPointUser.GetEndPointUser(User), request);
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new ProjectIssueListResponse
      {
        ProjectIssues = result.Value.Select(obj => new ProjectIssueRecord(obj.Category, obj.Date, obj.Description, obj.Effect, obj.Narration, obj.Id, obj.RegisteredBy, obj.Status, obj.SubCategory, obj.Title, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
