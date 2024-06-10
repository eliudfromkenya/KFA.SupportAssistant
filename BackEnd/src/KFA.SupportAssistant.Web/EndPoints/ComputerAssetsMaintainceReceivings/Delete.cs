using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Delete;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceReceivings;

/// <summary>
/// Delete a computer assets maintaince receiving.
/// </summary>
/// <remarks>
/// Delete a computer assets maintaince receiving by providing a valid receive id.
/// </remarks>
public class Delete(IMediator mediator, IEndPointManager endPointManager) : Endpoint<DeleteComputerAssetsMaintainceReceivingRequest>
{
  private const string EndPointId = "ENP-1D2";

  public override void Configure()
  {
    Delete(CoreFunctions.GetURL(DeleteComputerAssetsMaintainceReceivingRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Delete Computer Assets Maintaince Receiving End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Delete computer assets maintaince receiving";
      s.Description = "This endpoint is used to delete computer assets maintaince receiving with specified (provided) receive id(s)";
      s.ExampleRequest = new DeleteComputerAssetsMaintainceReceivingRequest { ReceiveID = "AAA-01,AAA-02" };
      s.ResponseExamples = new Dictionary<int, object> { { 204, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeleteComputerAssetsMaintainceReceivingRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.ReceiveID))
    {
      AddError(request => request.ReceiveID, "The receive id of the record to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<ComputerAssetsMaintainceReceiving>(CreateEndPointUser.GetEndPointUser(User), request.ReceiveID ?? "");
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    if (result.IsSuccess)
    {
      await SendNoContentAsync(cancellationToken);
    };
  }
}
