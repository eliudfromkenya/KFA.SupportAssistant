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

namespace KFA.SupportAssistant.Web.EndPoints.VendorCodesRequests;

/// <summary>
/// Update an existing vendor codes request.
/// </summary>
/// <remarks>
/// Update an existing vendor codes request by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateVendorCodesRequestRequest, UpdateVendorCodesRequestResponse>
{
  private const string EndPointId = "ENP-277";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateVendorCodesRequestRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Vendor Codes Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Vendor Codes Request";
      s.Description = "This endpoint is used to update  vendor codes request, making a full replacement of vendor codes request with a specifed valuse. A valid vendor codes request is required.";
      s.ExampleRequest = new UpdateVendorCodesRequestRequest { AttandedBy = "Attanded By", CostCentreCode = "Cost Centre Code", Description = "Description", Narration = "Narration", RequestingUser = "Requesting User", Status = "Status", TimeAttended = "Time Attended", TimeOfRequest = "Time of Request", VendorCode = "Vendor Code", VendorCodeRequestID = "1000", VendorType = "Vendor Type" };
      s.ResponseExamples[200] = new UpdateVendorCodesRequestResponse(new VendorCodesRequestRecord("Attanded By", "Cost Centre Code", "Description", "Narration", "Requesting User", "Status", DateTime.Now, DateTime.Now, "Vendor Code", "1000", "Vendor Type", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateVendorCodesRequestRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VendorCodeRequestID))
    {
      AddError(request => request.VendorCodeRequestID, "The vendor code request id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<VendorCodesRequestDTO, VendorCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.VendorCodeRequestID ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the vendor codes request to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<VendorCodesRequestDTO, VendorCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.VendorCodeRequestID ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateVendorCodesRequestResponse(new VendorCodesRequestRecord(obj.AttandedBy, obj.CostCentreCode, obj.Description, obj.Narration, obj.RequestingUser, obj.Status, obj.TimeAttended, obj.TimeOfRequest, obj.VendorCode, obj.Id, obj.VendorType, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
