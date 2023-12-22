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

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchQRCodesRequestRequest, QRCodesRequestRecord>
{
  private const string EndPointId = "ENP-1R6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchQRCodesRequestRequest.Route));
    //RequestBinder(new PatchBinder<QRCodesRequestDTO, QRCodesRequest, PatchQRCodesRequestRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update QR Codes Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a qr codes request";
      s.Description = "Used to update part of an existing qr codes request. A valid existing qr codes request is required.";
      s.ResponseExamples[200] = new QRCodesRequestRecord(string.Empty, true, "Narration", "1000", "Request Data", "Response Data", Core.DataLayer.Types.QRResponseType.Recieved, DateTime.Now, "Tims Machine used", "VAT Class", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchQRCodesRequestRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.QRCodeRequestID))
    {
      AddError(request => request.QRCodeRequestID, "The qr codes request of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    QRCodesRequestDTO patchFunc(QRCodesRequestDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<QRCodesRequestDTO, QRCodesRequest>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<QRCodesRequestDTO, QRCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.QRCodeRequestID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the qr codes request to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new QRCodesRequestRecord(obj.CostCentreCode, obj.IsDuplicate, obj.Narration, obj.Id, obj.RequestData, obj.ResponseData, obj.ResponseStatus, obj.Time, obj.TimsMachineUsed, obj.VATClass, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
