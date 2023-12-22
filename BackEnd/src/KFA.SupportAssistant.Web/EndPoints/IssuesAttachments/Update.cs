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

namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

/// <summary>
/// Update an existing issues attachment.
/// </summary>
/// <remarks>
/// Update an existing issues attachment by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateIssuesAttachmentRequest, UpdateIssuesAttachmentResponse>
{
  private const string EndPointId = "ENP-1E7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateIssuesAttachmentRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Issues Attachment End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Issues Attachment";
      s.Description = "This endpoint is used to update  issues attachment, making a full replacement of issues attachment with a specifed valuse. A valid issues attachment is required.";
      s.ExampleRequest = new UpdateIssuesAttachmentRequest { AttachmentID = "1000", AttachmentType = "Attachment Type", Data = new byte[] { }, Description = "Description", File = "File", IssueID = string.Empty, Narration = "Narration", Time = DateTime.Now };
      s.ResponseExamples[200] = new UpdateIssuesAttachmentResponse(new IssuesAttachmentRecord("1000", "Attachment Type", new byte[] { }, "Description", "File",string.Empty, "Narration", DateTime.Now, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateIssuesAttachmentRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AttachmentID))
    {
      AddError(request => request.AttachmentID, "The attachment id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<IssuesAttachmentDTO, IssuesAttachment>(CreateEndPointUser.GetEndPointUser(User), request.AttachmentID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the issues attachment to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<IssuesAttachmentDTO, IssuesAttachment>(CreateEndPointUser.GetEndPointUser(User), request.AttachmentID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateIssuesAttachmentResponse(new IssuesAttachmentRecord(obj.Id, obj.AttachmentType, obj.Data, obj.Description, obj.File, obj.IssueID, obj.Narration, obj.Time, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
