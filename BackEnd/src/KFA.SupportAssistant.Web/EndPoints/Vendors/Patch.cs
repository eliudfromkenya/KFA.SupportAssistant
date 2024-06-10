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

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchVendorRequest, VendorRecord>
{
  private const string EndPointId = "ENP-2L6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchVendorRequest.Route));
    //RequestBinder(new PatchBinder<VendorDTO, Vendor, PatchVendorRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Vendor End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a vendor";
      s.Description = "Used to update part of an existing vendor. A valid existing vendor is required.";
      s.ResponseExamples[200] = new VendorRecord("Contact", "Descriptions", "Email", true, "1000", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchVendorRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VendorCode))
    {
      AddError(request => request.VendorCode, "The vendor of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    VendorDTO patchFunc(VendorDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<VendorDTO, Vendor>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<VendorDTO, Vendor>(CreateEndPointUser.GetEndPointUser(User), request.VendorCode ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the vendor to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new VendorRecord(obj.Contact, obj.Descriptions, obj.Email, obj.IsActive, obj.Id, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
