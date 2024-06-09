using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

/// <summary>
/// Create a new ComputerAssetsMaintainceReceiving
/// </summary>
/// <remarks>
/// Creates a new computer assets maintaince receiving given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateComputerAssetsMaintainceReceivingRequest, CreateComputerAssetsMaintainceReceivingResponse>
{
  private const string EndPointId = "ENP-1D1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateComputerAssetsMaintainceReceivingRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Computer Assets Maintaince Receiving End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new computer assets maintaince receiving";
      s.Description = "This endpoint is used to create a new  computer assets maintaince receiving. Here details of computer assets maintaince receiving to be created is provided";
      s.ExampleRequest = new CreateComputerAssetsMaintainceReceivingRequest { AssetDetailID = "", BroughtBy = "Brought By", DateRecieved = DateTime.Now, Description = "Description", FromDepartmentCode = "From Department Code", Narration = "Narration", ReasonToSend = "Reason To Send", ReceiveID = "1000", RecievedBy = "Recieved By" };
      s.ResponseExamples[200] = new CreateComputerAssetsMaintainceReceivingResponse("", "Brought By", DateTime.Now, "Description", "From Department Code", "Narration", "Reason To Send", "1000", "Recieved By", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateComputerAssetsMaintainceReceivingRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<ComputerAssetsMaintainceReceivingDTO>();
    requestDTO.Id = request.ReceiveID;

    var result = await mediator.Send(new CreateModelCommand<ComputerAssetsMaintainceReceivingDTO, ComputerAssetsMaintainceReceiving>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);     
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is ComputerAssetsMaintainceReceivingDTO obj)
      {
        Response = new CreateComputerAssetsMaintainceReceivingResponse(obj.AssetDetailID, obj.BroughtBy, obj.DateRecieved, obj.Description, obj.FromDepartmentCode, obj.Narration, obj.ReasonToSend, obj.Id, obj.RecievedBy, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
