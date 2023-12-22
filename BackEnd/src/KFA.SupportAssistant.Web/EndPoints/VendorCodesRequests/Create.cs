using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.VendorCodesRequests;

/// <summary>
/// Create a new VendorCodesRequest
/// </summary>
/// <remarks>
/// Creates a new vendor codes request given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateVendorCodesRequestRequest, CreateVendorCodesRequestResponse>
{
  private const string EndPointId = "ENP-271";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateVendorCodesRequestRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Vendor Codes Request End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new vendor codes request";
      s.Description = "This endpoint is used to create a new  vendor codes request. Here details of vendor codes request to be created is provided";
      s.ExampleRequest = new CreateVendorCodesRequestRequest { AttandedBy = "Attanded By", CostCentreCode = "Cost Centre Code", Description = "Description", Narration = "Narration", RequestingUser = "Requesting User", Status = "Status", TimeAttended = "Time Attended", TimeOfRequest = "Time of Request", VendorCode = "Vendor Code", VendorCodeRequestID = "1000", VendorType = "Vendor Type" };
      s.ResponseExamples[200] = new CreateVendorCodesRequestResponse("Attanded By", "Cost Centre Code", "Description", "Narration", "Requesting User", "Status", DateTime.Now, DateTime.Now, "Vendor Code", "1000", "Vendor Type", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateVendorCodesRequestRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<VendorCodesRequestDTO>();
    requestDTO.Id = request.VendorCodeRequestID;

    var result = await mediator.Send(new CreateModelCommand<VendorCodesRequestDTO, VendorCodesRequest>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is VendorCodesRequestDTO obj)
      {
        Response = new CreateVendorCodesRequestResponse(obj.AttandedBy, obj.CostCentreCode, obj.Description, obj.Narration, obj.RequestingUser, obj.Status, obj.TimeAttended, obj.TimeOfRequest, obj.VendorCode, obj.Id, obj.VendorType, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
