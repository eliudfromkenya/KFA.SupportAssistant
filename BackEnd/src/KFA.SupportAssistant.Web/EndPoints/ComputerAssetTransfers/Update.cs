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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

/// <summary>
/// Update an existing computer asset transfer.
/// </summary>
/// <remarks>
/// Update an existing computer asset transfer by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateComputerAssetTransferRequest, UpdateComputerAssetTransferResponse>
{
  private const string EndPointId = "ENP-197";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateComputerAssetTransferRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Computer Asset Transfer End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Computer Asset Transfer";
      s.Description = "This endpoint is used to update  computer asset transfer, making a full replacement of computer asset transfer with a specifed valuse. A valid computer asset transfer is required.";
      s.ExampleRequest = new UpdateComputerAssetTransferRequest { AssetDetailID = "", CostCentreCode = "Cost Centre Code", Narration = "Narration", PayrollNumber = "Payroll Number", ResponsibleUser = "Responsible User", Status = "Status", TransferDate = DateTime.Now, TransferID = "1000", TransferReasons = "Transfer Reasons" };
      s.ResponseExamples[200] = new UpdateComputerAssetTransferResponse (new ComputerAssetTransferRecord("", "Cost Centre Code", "Narration", "Payroll Number", "Responsible User", "Status", DateTime.Now, "1000", "Transfer Reasons", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateComputerAssetTransferRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.TransferID))
    {
      AddError(request => request.TransferID , "The transfer id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<ComputerAssetTransferDTO, ComputerAssetTransfer>(CreateEndPointUser.GetEndPointUser(User), request.TransferID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the computer asset transfer to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<ComputerAssetTransferDTO, ComputerAssetTransfer>(CreateEndPointUser.GetEndPointUser(User), request.TransferID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateComputerAssetTransferResponse(new ComputerAssetTransferRecord(obj.AssetDetailID, obj.CostCentreCode, obj.Narration, obj.PayrollNumber, obj.ResponsibleUser, obj.Status, obj.TransferDate, obj.Id, obj.TransferReasons, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
