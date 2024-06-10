using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsWriteOffs;

/// <summary>
/// Get a computer assets write off by write off id.
/// </summary>
/// <remarks>
/// Takes write off id and returns a matching computer assets write off record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetsWriteOffByIdRequest, ComputerAssetsWriteOffRecord>
{
  private const string EndPointId = "ENP-1G4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetsWriteOffByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Assets Write Off End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer assets write off by specified write off id";
      s.Description = "This endpoint is used to retrieve computer assets write off with the provided write off id";
      s.ExampleRequest = new GetComputerAssetsWriteOffByIdRequest { WriteOffID = "write off id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetsWriteOffRecord("", "Description", "Narration", "Reason For Write-Off", "1000", DateTime.Now, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetsWriteOffByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.WriteOffID))
    {
      AddError(request => request.WriteOffID, "The write off id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetsWriteOffDTO, ComputerAssetsWriteOff>(CreateEndPointUser.GetEndPointUser(User), request.WriteOffID ?? "");
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
      Response = new ComputerAssetsWriteOffRecord(obj.AssetDetailID, obj.Description, obj.Narration, obj.ReasonForWriteOff, obj.Id, obj.WriteOffDate, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
