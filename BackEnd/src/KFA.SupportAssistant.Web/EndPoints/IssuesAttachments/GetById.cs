using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

/// <summary>
/// Get a issues attachment by attachment id.
/// </summary>
/// <remarks>
/// Takes attachment id and returns a matching issues attachment record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetIssuesAttachmentByIdRequest, IssuesAttachmentRecord>
{
  private const string EndPointId = "ENP-1E4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetIssuesAttachmentByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Issues Attachment End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets issues attachment by specified attachment id";
      s.Description = "This endpoint is used to retrieve issues attachment with the provided attachment id";
      s.ExampleRequest = new GetIssuesAttachmentByIdRequest { AttachmentID = "attachment id to retrieve" };
      s.ResponseExamples[200] = new IssuesAttachmentRecord("1000", "Attachment Type", new byte[] { }, "Description", "File", string.Empty, "Narration", DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetIssuesAttachmentByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AttachmentID))
    {
      AddError(request => request.AttachmentID, "The attachment id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<IssuesAttachmentDTO, IssuesAttachment>(CreateEndPointUser.GetEndPointUser(User), request.AttachmentID ?? "");
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
      Response = new IssuesAttachmentRecord(obj.Id, obj.AttachmentType, obj.Data, obj.Description, obj.File, obj.IssueID, obj.Narration, obj.Time, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
