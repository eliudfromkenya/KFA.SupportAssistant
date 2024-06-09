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

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

/// <summary>
/// Update an existing vendor.
/// </summary>
/// <remarks>
/// Update an existing vendor by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateVendorRequest, UpdateVendorResponse>
{
  private const string EndPointId = "ENP-2L7";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateVendorRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update Vendor End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full Vendor";
      s.Description = "This endpoint is used to update  vendor, making a full replacement of vendor with a specifed valuse. A valid vendor is required.";
      s.ExampleRequest = new UpdateVendorRequest { Contact = "Contact", Descriptions = "Descriptions", Email = "Email", IsActive = true, VendorCode = "1000" };
      s.ResponseExamples[200] = new UpdateVendorResponse (new VendorRecord("Contact", "Descriptions", "Email", true, "1000", DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateVendorRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VendorCode))
    {
      AddError(request => request.VendorCode , "The vendor code of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<VendorDTO, Vendor>(CreateEndPointUser.GetEndPointUser(User), request.VendorCode ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the vendor to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<VendorDTO, Vendor>(CreateEndPointUser.GetEndPointUser(User), request.VendorCode ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateVendorResponse(new VendorRecord(obj.Contact, obj.Descriptions, obj.Email, obj.IsActive, obj.Id, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}