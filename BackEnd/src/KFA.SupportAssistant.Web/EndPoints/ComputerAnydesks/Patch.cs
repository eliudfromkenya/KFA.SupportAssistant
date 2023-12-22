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

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAnydesks;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchComputerAnydeskRequest, ComputerAnydeskRecord>
{
  private const string EndPointId = "ENP-146";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchComputerAnydeskRequest.Route));
    //RequestBinder(new PatchBinder<ComputerAnydeskDTO, ComputerAnydesk, PatchComputerAnydeskRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Computer Anydesk End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a computer anydesk";
      s.Description = "Used to update part of an existing computer anydesk. A valid existing computer anydesk is required.";
      s.ResponseExamples[200] = new ComputerAnydeskRecord("1000", "AnyDesk Number", "Cost Centre Code", "Device Name", "Name Of User", "Narration", "Password", Core.DataLayer.Types.AnyDeskComputerType.Sales, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchComputerAnydeskRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.AnyDeskId))
    {
      AddError(request => request.AnyDeskId, "The computer anydesk of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    ComputerAnydeskDTO patchFunc(ComputerAnydeskDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<ComputerAnydeskDTO, ComputerAnydesk>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<ComputerAnydeskDTO, ComputerAnydesk>(CreateEndPointUser.GetEndPointUser(User), request.AnyDeskId ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the computer anydesk to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new ComputerAnydeskRecord(obj.Id, obj.AnyDeskNumber, obj.CostCentreCode, obj.DeviceName, obj.NameOfUser, obj.Narration, obj.Password, obj.Type, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
