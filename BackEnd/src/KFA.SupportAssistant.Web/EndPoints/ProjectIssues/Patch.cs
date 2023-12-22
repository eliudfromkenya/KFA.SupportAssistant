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

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchProjectIssueRequest, ProjectIssueRecord>
{
  private const string EndPointId = "ENP-1O6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchProjectIssueRequest.Route));
    //RequestBinder(new PatchBinder<ProjectIssueDTO, ProjectIssue, PatchProjectIssueRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Project Issue End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a project issue";
      s.Description = "Used to update part of an existing project issue. A valid existing project issue is required.";
      s.ResponseExamples[200] = new ProjectIssueRecord("Category", DateTime.Now, "Description", "Effect", "Narration", "1000", "Registered By", 0, "Sub Category", "Title", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchProjectIssueRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ProjectIssueID))
    {
      AddError(request => request.ProjectIssueID, "The project issue of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ProjectIssueDTO patchFunc(ProjectIssueDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ProjectIssueDTO, ProjectIssue>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ProjectIssueDTO, ProjectIssue>(CreateEndPointUser.GetEndPointUser(User), request.ProjectIssueID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the project issue to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ProjectIssueRecord(obj.Category, obj.Date, obj.Description, obj.Effect, obj.Narration, obj.Id, obj.RegisteredBy, obj.Status, obj.SubCategory, obj.Title, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
