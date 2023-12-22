using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

/// <summary>
/// Get a qr codes request by qr code request id.
/// </summary>
/// <remarks>
/// Takes qr code request id and returns a matching qr codes request record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetQRCodesRequestByIdRequest, QRCodesRequestRecord>
{
  private const string EndPointId = "ENP-1R4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetQRCodesRequestByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get QR Codes Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets qr codes request by specified qr code request id";
      s.Description = "This endpoint is used to retrieve qr codes request with the provided qr code request id";
      s.ExampleRequest = new GetQRCodesRequestByIdRequest { QRCodeRequestID = "qr code request id to retrieve" };
      s.ResponseExamples[200] = new QRCodesRequestRecord(string.Empty, true, "Narration", "1000", "Request Data", "Response Data",  Core.DataLayer.Types.QRResponseType.Recieved, DateTime.Now, "Tims Machine used", "VAT Class", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetQRCodesRequestByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.QRCodeRequestID))
    {
      AddError(request => request.QRCodeRequestID, "The qr code request id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<QRCodesRequestDTO, QRCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.QRCodeRequestID ?? "");
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
      Response = new QRCodesRequestRecord(obj.CostCentreCode, obj.IsDuplicate, obj.Narration, obj.Id, obj.RequestData, obj.ResponseData, obj.ResponseStatus, obj.Time, obj.TimsMachineUsed, obj.VATClass, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
