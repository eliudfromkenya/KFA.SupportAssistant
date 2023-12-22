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

namespace KFA.SupportAssistant.Web.EndPoints.VendorCodesRequests;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchVendorCodesRequestRequest, VendorCodesRequestRecord>
{
  private const string EndPointId = "ENP-276";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchVendorCodesRequestRequest.Route));
    //RequestBinder(new PatchBinder<VendorCodesRequestDTO, VendorCodesRequest, PatchVendorCodesRequestRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Vendor Codes Request End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a vendor codes request";
      s.Description = "Used to update part of an existing vendor codes request. A valid existing vendor codes request is required.";
      s.ResponseExamples[200] = new VendorCodesRequestRecord("Attanded By", "Cost Centre Code", "Description", "Narration", "Requesting User", "Status", DateTime.Now, DateTime.Now, "Vendor Code", "1000", "Vendor Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchVendorCodesRequestRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VendorCodeRequestID))
    {
      AddError(request => request.VendorCodeRequestID, "The vendor codes request of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    VendorCodesRequestDTO patchFunc(VendorCodesRequestDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<VendorCodesRequestDTO, VendorCodesRequest>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<VendorCodesRequestDTO, VendorCodesRequest>(CreateEndPointUser.GetEndPointUser(User), request.VendorCodeRequestID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the vendor codes request to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new VendorCodesRequestRecord(obj.AttandedBy, obj.CostCentreCode, obj.Description, obj.Narration, obj.RequestingUser, obj.Status, obj.TimeAttended, obj.TimeOfRequest, obj.VendorCode, obj.Id, obj.VendorType, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
