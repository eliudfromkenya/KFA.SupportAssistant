using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

/// <summary>
/// Create a new ComputerAssetTransfer
/// </summary>
/// <remarks>
/// Creates a new computer asset transfer given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetTransferRequest, CreateComputerAssetTransferResponse>
{
  private const string EndPointId = "ENP-191";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetTransferRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Asset Transfer End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer asset transfer";
      s.Description = "This endpoint is used to create a new  computer asset transfer. Here details of computer asset transfer to be created is provided";
      s.ExampleRequest = new CreateComputerAssetTransferRequest { AssetDetailID = "", CostCentreCode = "Cost Centre Code", Narration = "Narration", PayrollNumber = "Payroll Number", ResponsibleUser = "Responsible User", Status = "Status", TransferDate = DateTime.Now, TransferID = "1000", TransferReasons = "Transfer Reasons" };
      s.ResponseExamples[200] = new CreateComputerAssetTransferResponse("", "Cost Centre Code", "Narration", "Payroll Number", "Responsible User", "Status", DateTime.Now, "1000", "Transfer Reasons", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetTransferRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetTransferDTO>();
    requestDTO.Id = request.TransferID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetTransferDTO, ComputerAssetTransfer>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetTransferDTO obj)
      {
        Response = new CreateComputerAssetTransferResponse(obj.AssetDetailID, obj.CostCentreCode, obj.Narration, obj.PayrollNumber, obj.ResponsibleUser, obj.Status, obj.TransferDate, obj.Id, obj.TransferReasons, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
