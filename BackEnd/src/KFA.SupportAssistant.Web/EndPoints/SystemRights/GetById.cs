using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.SystemRights;

/// <summary>
/// Get a system right by right id.
/// </summary>
/// <remarks>
/// Takes right id and returns a matching system right record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetSystemRightByIdRequest, SystemRightRecord>
{
  private const string EndPointId = "ENP-204";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetSystemRightByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get System Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets system right by specified right id";
      s.Description = "This endpoint is used to retrieve system right with the provided right id";
      s.ExampleRequest = new GetSystemRightByIdRequest { RightId = "right id to retrieve" };
      s.ResponseExamples[200] = new SystemRightRecord(true, "Narration", "1000", "Right Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetSystemRightByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RightId))
    {
      AddError(request => request.RightId, "The right id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<SystemRightDTO, SystemRight>(CreateEndPointUser.GetEndPointUser(User), request.RightId ?? "");
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
      Response = new SystemRightRecord(obj.IsCompulsory, obj.Narration, obj.Id, obj.RightName, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
