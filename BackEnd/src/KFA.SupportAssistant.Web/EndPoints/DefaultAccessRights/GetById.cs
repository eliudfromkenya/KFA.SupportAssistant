using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.DefaultAccessRights;

/// <summary>
/// Get a default access right by right id.
/// </summary>
/// <remarks>
/// Takes right id and returns a matching default access right record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetDefaultAccessRightByIdRequest, DefaultAccessRightRecord>
{
  private const string EndPointId = "ENP-184";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetDefaultAccessRightByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Default Access Right End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets default access right by specified right id";
      s.Description = "This endpoint is used to retrieve default access right with the provided right id";
      s.ExampleRequest = new GetDefaultAccessRightByIdRequest { RightID = "right id to retrieve" };
      s.ResponseExamples[200] = new DefaultAccessRightRecord("Name", "Narration", "1000", "Rights", "Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetDefaultAccessRightByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.RightID))
    {
      AddError(request => request.RightID, "The right id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<DefaultAccessRightDTO, DefaultAccessRight>(CreateEndPointUser.GetEndPointUser(User), request.RightID ?? "");
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
      Response = new DefaultAccessRightRecord(obj.Name, obj.Narration, obj.Id, obj.Rights, obj.Type, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
