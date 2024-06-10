using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

/// <summary>
/// Get a vendor by vendor code.
/// </summary>
/// <remarks>
/// Takes vendor code and returns a matching vendor record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetVendorByIdRequest, VendorRecord>
{
  private const string EndPointId = "ENP-2L4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetVendorByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Vendor End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets vendor by specified vendor code";
      s.Description = "This endpoint is used to retrieve vendor with the provided vendor code";
      s.ExampleRequest = new GetVendorByIdRequest { VendorCode = "vendor code to retrieve" };
      s.ResponseExamples[200] = new VendorRecord("Contact", "Descriptions", "Email", true, "1000", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetVendorByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VendorCode))
    {
      AddError(request => request.VendorCode, "The vendor code of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<VendorDTO, Vendor>(CreateEndPointUser.GetEndPointUser(User), request.VendorCode ?? "");
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
      Response = new VendorRecord(obj.Contact, obj.Descriptions, obj.Email, obj.IsActive, obj.Id, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
