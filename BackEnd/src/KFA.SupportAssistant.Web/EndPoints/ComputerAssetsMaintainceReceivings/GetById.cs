
using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

/// <summary>
/// Get a computer assets maintaince receiving by receive id.
/// </summary>
/// <remarks>
/// Takes receive id and returns a matching computer assets maintaince receiving record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetsMaintainceReceivingByIdRequest, ComputerAssetsMaintainceReceivingRecord>
{
  private const string EndPointId = "ENP-1D4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetsMaintainceReceivingByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Assets Maintaince Receiving End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer assets maintaince receiving by specified receive id";
      s.Description = "This endpoint is used to retrieve computer assets maintaince receiving with the provided receive id";
      s.ExampleRequest = new GetComputerAssetsMaintainceReceivingByIdRequest { ReceiveID = "receive id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetsMaintainceReceivingRecord("", "Brought By", DateTime.Now, "Description", "From Department Code", "Narration", "Reason To Send", "1000", "Recieved By", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetsMaintainceReceivingByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ReceiveID))
    {
      AddError(request => request.ReceiveID, "The receive id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetsMaintainceReceivingDTO, ComputerAssetsMaintainceReceiving>(CreateEndPointUser.GetEndPointUser(User), request.ReceiveID ?? "");
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
      Response = new ComputerAssetsMaintainceReceivingRecord(obj.AssetDetailID, obj.BroughtBy, obj.DateRecieved, obj.Description, obj.FromDepartmentCode, obj.Narration, obj.ReasonToSend, obj.Id, obj.RecievedBy, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
