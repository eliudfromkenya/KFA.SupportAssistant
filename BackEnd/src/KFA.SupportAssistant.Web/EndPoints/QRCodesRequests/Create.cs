using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

/// <summary>
/// Create a new QRCodesRequest
/// </summary>
/// <remarks>
/// Creates a new qr codes request given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateQRCodesRequestRequest, CreateQRCodesRequestResponse>
{
  private const string EndPointId = "ENP-1R1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateQRCodesRequestRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add QR Codes Request End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new qr codes request";
      s.Description = "This endpoint is used to create a new  qr codes request. Here details of qr codes request to be created is provided";
      s.ExampleRequest = new CreateQRCodesRequestRequest { CostCentreCode = string.Empty, IsDuplicate = true, Narration = "Narration", QRCodeRequestID = "1000", RequestData = "Request Data", ResponseData = "Response Data", ResponseStatus = Core.DataLayer.Types.QRResponseType.Recieved, Time = DateTime.Now, TimsMachineused = "Tims Machine used", VATClass = "VAT Class" };
      s.ResponseExamples[200] = new CreateQRCodesRequestResponse(string.Empty, true, "Narration", "1000", "Request Data", "Response Data", Core.DataLayer.Types.QRResponseType.Recieved, DateTime.Now, "Tims Machine used", "VAT Class", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateQRCodesRequestRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<QRCodesRequestDTO>();
    requestDTO.Id = request.QRCodeRequestID;

    var result = await mediator.Send(new CreateModelCommand<QRCodesRequestDTO, QRCodesRequest>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is QRCodesRequestDTO obj)
      {
        Response = new CreateQRCodesRequestResponse(obj.CostCentreCode, obj.IsDuplicate, obj.Narration, obj.Id, obj.RequestData, obj.ResponseData, obj.ResponseStatus, obj.Time, obj.TimsMachineUsed, obj.VATClass, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
