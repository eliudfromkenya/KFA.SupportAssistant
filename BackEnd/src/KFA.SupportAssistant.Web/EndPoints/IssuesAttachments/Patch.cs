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

namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchIssuesAttachmentRequest, IssuesAttachmentRecord>
{
  private const string EndPointId = "ENP-1E6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchIssuesAttachmentRequest.Route));
    //RequestBinder(new PatchBinder<IssuesAttachmentDTO, IssuesAttachment, PatchIssuesAttachmentRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Issues Attachment End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a issues attachment";
      s.Description = "Used to update part of an existing issues attachment. A valid existing issues attachment is required.";
      s.ResponseExamples[200] = new IssuesAttachmentRecord("1000", "Attachment Type", new byte[] { }, "Description", "File", string.Empty, "Narration", DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchIssuesAttachmentRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AttachmentID))
    {
      AddError(request => request.AttachmentID, "The issues attachment of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    IssuesAttachmentDTO patchFunc(IssuesAttachmentDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<IssuesAttachmentDTO, IssuesAttachment>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<IssuesAttachmentDTO, IssuesAttachment>(CreateEndPointUser.GetEndPointUser(User), request.AttachmentID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the issues attachment to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new IssuesAttachmentRecord(obj.Id, obj.AttachmentType, obj.Data, obj.Description, obj.File, obj.IssueID, obj.Narration, obj.Time, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
