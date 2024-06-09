using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

/// <summary>
/// Create a new Vendor
/// </summary>
/// <remarks>
/// Creates a new vendor given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateVendorRequest, CreateVendorResponse>
{
  private const string EndPointId = "ENP-2L1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateVendorRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Vendor End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new vendor";
      s.Description = "This endpoint is used to create a new  vendor. Here details of vendor to be created is provided";
      s.ExampleRequest = new CreateVendorRequest { Contact = "Contact", Descriptions = "Descriptions", Email = "Email", IsActive = true, VendorCode = "1000" };
      s.ResponseExamples[200] = new CreateVendorResponse("Contact", "Descriptions", "Email", true, "1000", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateVendorRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<VendorDTO>();
    requestDTO.Id = request.VendorCode;

    var result = await mediator.Send(new CreateModelCommand<VendorDTO, Vendor>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);     
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is VendorDTO obj)
      {
        Response = new CreateVendorResponse(obj.Contact, obj.Descriptions, obj.Email, obj.IsActive, obj.Id, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
