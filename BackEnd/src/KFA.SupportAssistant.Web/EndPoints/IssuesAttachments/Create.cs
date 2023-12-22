using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

/// <summary>
/// Create a new IssuesAttachment
/// </summary>
/// <remarks>
/// Creates a new issues attachment given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateIssuesAttachmentRequest, CreateIssuesAttachmentResponse>
{
  private const string EndPointId = "ENP-1E1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateIssuesAttachmentRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Issues Attachment End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new issues attachment";
      s.Description = "This endpoint is used to create a new  issues attachment. Here details of issues attachment to be created is provided";
      s.ExampleRequest = new CreateIssuesAttachmentRequest { AttachmentID = "1000", AttachmentType = "Attachment Type", Data = new byte[] { }, Description = "Description", File = "File", IssueID = string.Empty, Narration = "Narration", Time = DateTime.Now };
      s.ResponseExamples[200] = new CreateIssuesAttachmentResponse("1000", "Attachment Type", new byte[] { }, "Description", "File", string.Empty, "Narration", DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateIssuesAttachmentRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<IssuesAttachmentDTO>();
    requestDTO.Id = request.AttachmentID;

    var result = await mediator.Send(new CreateModelCommand<IssuesAttachmentDTO, IssuesAttachment>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is IssuesAttachmentDTO obj)
      {
        Response = new CreateIssuesAttachmentResponse(obj.Id, obj.AttachmentType, obj.Data, obj.Description, obj.File, obj.IssueID, obj.Narration, obj.Time, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
