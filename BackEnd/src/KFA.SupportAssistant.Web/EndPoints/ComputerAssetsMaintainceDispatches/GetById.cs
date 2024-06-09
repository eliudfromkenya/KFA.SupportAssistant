
using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceDispatches;

/// <summary>
/// Get a computer assets maintaince dispatch by dispatch id.
/// </summary>
/// <remarks>
/// Takes dispatch id and returns a matching computer assets maintaince dispatch record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetsMaintainceDispatchByIdRequest, ComputerAssetsMaintainceDispatchRecord>
{
  private const string EndPointId = "ENP-1C4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetsMaintainceDispatchByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Assets Maintaince Dispatch End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer assets maintaince dispatch by specified dispatch id";
      s.Description = "This endpoint is used to retrieve computer assets maintaince dispatch with the provided dispatch id";
      s.ExampleRequest = new GetComputerAssetsMaintainceDispatchByIdRequest { DispatchID = "dispatch id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetsMaintainceDispatchRecord("", "Collected By", DateTime.Now, "Description", "1000", "Narration", "Reason To Send", "Sent By", "To Department Code", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetsMaintainceDispatchByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.DispatchID))
    {
      AddError(request => request.DispatchID, "The dispatch id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetsMaintainceDispatchDTO, ComputerAssetsMaintainceDispatch>(CreateEndPointUser.GetEndPointUser(User), request.DispatchID ?? "");
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
      Response = new ComputerAssetsMaintainceDispatchRecord(obj.AssetDetailID, obj.CollectedBy, obj.DateSend, obj.Description, obj.Id, obj.Narration, obj.ReasonToSend, obj.SentBy, obj.ToDepartmentCode, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
