using Ardalis.Result;
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

namespace KFA.SupportAssistant.Web.EndPoints.IssuesProgresses;

/// <summary>
/// List all issues progresses by specified conditions
/// </summary>
/// <remarks>
/// List all issues progresses - returns a IssuesProgressListResponse containing the issues progresses.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, IssuesProgressListResponse>
{
  private const string EndPointId = "ENP-1F5";
  public const string Route = "/issues_progresses";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Issues Progresses List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of issues progresses as specified";
      s.Description = "Returns all issues progresses as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new IssuesProgressListResponse { IssuesProgresses = [new IssuesProgressRecord("Description", "", "Narration", "1000", "Reported By",  Core.Models.Types.IssueStatus.Pending, DateTime.Now, DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<IssuesProgressDTO, IssuesProgress>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<IssuesProgressDTO>>.Success(ans.Select(v => (IssuesProgressDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new IssuesProgressListResponse
      {
        IssuesProgresses = result.Value.Select(obj => new IssuesProgressRecord(obj.Description, obj.IssueID, obj.Narration, obj.Id, obj.ReportedBy, obj.Status, obj.Time, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
