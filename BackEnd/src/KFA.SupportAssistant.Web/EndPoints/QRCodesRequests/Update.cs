using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DataLayer.Types;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

/// <summary>
/// Update an existing qr codes request.
/// </summary>
/// <remarks>
/// Update an existing qr codes request by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateQRCodesRequestRequest, UpdateQRCodesRequestResponse>
{
  private const string EndPointId = "ENP-1R7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateQRCodesRequestRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update QR Codes Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full QR Codes Request";
      s.Description = "This endpoint is used to update  qr codes request, making a full replacement of qr codes request with a specifed valuse. A valid qr codes request is required.";
      s.ExampleRequest = new UpdateQRCodesRequestRequest { CostCentreCode = string.Empty, IsDuplicate = true, Narration = "Narration", QRCodeRequestID = "1000", RequestData = "Request Data", ResponseData = "Response Data", ResponseStatus = "Response Status", Time = DateTime.Now, TimsMachineused = "Tims Machine used", VATClass = "VAT Class" };
      s.ResponseExamples[200] = new UpdateQRCodesRequestResponse(new QRCodesRequestRecord(string.Empty, true, "Narration", "1000", "Request Data", "Response Data", QRResponseType.Recieved, DateTime.Now, "Tims Machine used", "VAT Class", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateQRCodesRequestRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.QRCodeRequestID))
    {
      AddError(request => request.QRCodeRequestID, "The qr code request id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<QRCodesRequestDTO, QRCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.QRCodeRequestID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the qr codes request to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<QRCodesRequestDTO, QRCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.QRCodeRequestID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateQRCodesRequestResponse(new QRCodesRequestRecord(obj.CostCentreCode, obj.IsDuplicate, obj.Narration, obj.Id, obj.RequestData, obj.ResponseData, obj.ResponseStatus, obj.Time, obj.TimsMachineUsed, obj.VATClass, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
