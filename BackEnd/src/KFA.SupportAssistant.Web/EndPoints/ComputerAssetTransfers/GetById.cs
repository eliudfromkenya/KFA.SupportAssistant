using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

/// <summary>
/// Get a computer asset transfer by transfer id.
/// </summary>
/// <remarks>
/// Takes transfer id and returns a matching computer asset transfer record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetComputerAssetTransferByIdRequest, ComputerAssetTransferRecord>
{
  private const string EndPointId = "ENP-194";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetComputerAssetTransferByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Computer Asset Transfer End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets computer asset transfer by specified transfer id";
      s.Description = "This endpoint is used to retrieve computer asset transfer with the provided transfer id";
      s.ExampleRequest = new GetComputerAssetTransferByIdRequest { TransferID = "transfer id to retrieve" };
      s.ResponseExamples[200] = new ComputerAssetTransferRecord("", "Cost Centre Code", "Narration", "Payroll Number", "Responsible User", "Status", DateTime.Now, "1000", "Transfer Reasons", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetComputerAssetTransferByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.TransferID))
    {
      AddError(request => request.TransferID, "The transfer id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<ComputerAssetTransferDTO, ComputerAssetTransfer>(CreateEndPointUser.GetEndPointUser(User), request.TransferID ?? "");
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
      Response = new ComputerAssetTransferRecord(obj.AssetDetailID, obj.CostCentreCode, obj.Narration, obj.PayrollNumber, obj.ResponsibleUser, obj.Status, obj.TransferDate, obj.Id, obj.TransferReasons, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
