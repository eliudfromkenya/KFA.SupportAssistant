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

namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchSupplierRequest, SupplierRecord>
{
  private const string EndPointId = "ENP-1Z6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchSupplierRequest.Route));
    //RequestBinder(new PatchBinder<SupplierDTO, Supplier, PatchSupplierRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Supplier End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a supplier";
      s.Description = "Used to update part of an existing supplier. A valid existing supplier is required.";
      s.ResponseExamples[200] = new SupplierRecord("Address", "Contact Person", "Cost Centre Code", "Description", "Email", "Narration", "Postal Code", "Supplier Code", "1000", "Telephone", "Town", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchSupplierRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SupplierId))
    {
      AddError(request => request.SupplierId, "The supplier of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    SupplierDTO patchFunc(SupplierDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<SupplierDTO, Supplier>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<SupplierDTO, Supplier>(CreateEndPointUser.GetEndPointUser(User), request.SupplierId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the supplier to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new SupplierRecord(obj.Address, obj.ContactPerson, obj.CostCentreCode, obj.Description, obj.Email, obj.Narration, obj.PostalCode, obj.SupplierCode, obj.Id, obj.Telephone, obj.Town, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
