using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.Suppliers;

/// <summary>
/// Create a new Supplier
/// </summary>
/// <remarks>
/// Creates a new supplier given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateSupplierRequest, CreateSupplierResponse>
{
  private const string EndPointId = "ENP-1Z1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateSupplierRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Supplier End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new supplier";
      s.Description = "This endpoint is used to create a new  supplier. Here details of supplier to be created is provided";
      s.ExampleRequest = new CreateSupplierRequest { Address = "Address", ContactPerson = "Contact Person", CostCentreCode = "Cost Centre Code", Description = "Description", Email = "Email", Narration = "Narration", PostalCode = "Postal Code", SupplierCode = "Supplier Code", SupplierId = "1000", Telephone = "Telephone", Town = "Town" };
      s.ResponseExamples[200] = new CreateSupplierResponse("Address", "Contact Person", "Cost Centre Code", "Description", "Email", "Narration", "Postal Code", "Supplier Code", "1000", "Telephone", "Town", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateSupplierRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<SupplierDTO>();
    requestDTO.Id = request.SupplierId;

    var result = await mediator.Send(new CreateModelCommand<SupplierDTO, Supplier>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is SupplierDTO obj)
      {
        Response = new CreateSupplierResponse(obj.Address, obj.ContactPerson, obj.CostCentreCode, obj.Description, obj.Email, obj.Narration, obj.PostalCode, obj.SupplierCode, obj.Id, obj.Telephone, obj.Town, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
