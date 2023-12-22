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

namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

/// <summary>
/// Update an existing supplier.
/// </summary>
/// <remarks>
/// Update an existing supplier by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateSupplierRequest, UpdateSupplierResponse>
{
  private const string EndPointId = "ENP-1Z7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateSupplierRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Supplier End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Supplier";
      s.Description = "This endpoint is used to update  supplier, making a full replacement of supplier with a specifed valuse. A valid supplier is required.";
      s.ExampleRequest = new UpdateSupplierRequest { Address = "Address", ContactPerson = "Contact Person", CostCentreCode = "Cost Centre Code", Description = "Description", Email = "Email", Narration = "Narration", PostalCode = "Postal Code", SupplierCode = "Supplier Code", SupplierId = "1000", Telephone = "Telephone", Town = "Town" };
      s.ResponseExamples[200] = new UpdateSupplierResponse(new SupplierRecord("Address", "Contact Person", "Cost Centre Code", "Description", "Email", "Narration", "Postal Code", "Supplier Code", "1000", "Telephone", "Town", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateSupplierRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.SupplierId))
    {
      AddError(request => request.SupplierId, "The supplier id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<SupplierDTO, Supplier>(CreateEndPointUser.GetEndPointUser(User), request.SupplierId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the supplier to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<SupplierDTO, Supplier>(CreateEndPointUser.GetEndPointUser(User), request.SupplierId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateSupplierResponse(new SupplierRecord(obj.Address, obj.ContactPerson, obj.CostCentreCode, obj.Description, obj.Email, obj.Narration, obj.PostalCode, obj.SupplierCode, obj.Id, obj.Telephone, obj.Town, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
