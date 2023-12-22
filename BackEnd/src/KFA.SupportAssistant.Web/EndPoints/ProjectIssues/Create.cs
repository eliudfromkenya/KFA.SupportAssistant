using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ProjectIssues;

/// <summary>
/// Create a new ProjectIssue
/// </summary>
/// <remarks>
/// Creates a new project issue given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateProjectIssueRequest, CreateProjectIssueResponse>
{
  private const string EndPointId = "ENP-1O1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateProjectIssueRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Project Issue End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new project issue";
      s.Description = "This endpoint is used to create a new  project issue. Here details of project issue to be created is provided";
      s.ExampleRequest = new CreateProjectIssueRequest { Category = "Category", Date = DateTime.Now, Description = "Description", Effect = "Effect", Narration = "Narration", ProjectIssueID = "1000", RegisteredBy = "Registered By", Status = 0, SubCategory = "Sub Category", Title = "Title" };
      s.ResponseExamples[200] = new CreateProjectIssueResponse("Category", DateTime.Now, "Description", "Effect", "Narration", "1000", "Registered By", 0, "Sub Category", "Title", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateProjectIssueRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ProjectIssueDTO>();
    requestDTO.Id = request.ProjectIssueID;

    var result = await mediator.Send(new CreateModelCommand<ProjectIssueDTO, ProjectIssue>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ProjectIssueDTO obj)
      {
        Response = new CreateProjectIssueResponse(obj.Category, obj.Date, obj.Description, obj.Effect, obj.Narration, obj.Id, obj.RegisteredBy, obj.Status, obj.SubCategory, obj.Title, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
