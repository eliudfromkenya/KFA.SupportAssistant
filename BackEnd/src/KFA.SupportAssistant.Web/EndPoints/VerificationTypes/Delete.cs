using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Delete;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

/// <summary>
/// Delete a verification type.
/// </summary>
/// <remarks>
/// Delete a verification type by providing a valid verification type id.
/// </remarks>
public class Delete(IMediator mediator, IEndPointManager endPointManager) : Endpoint<DeleteVerificationTypeRequest>
{
  private const string EndPointId = "ENP-292";

  public override void Configure()
  {
    Delete(CoreFunctions.GetURL(DeleteVerificationTypeRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Delete Verification Type End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Delete verification type";
      s.Description = "This endpoint is used to delete verification type with specified (provided) verification type id(s)";
      s.ExampleRequest = new DeleteVerificationTypeRequest { VerificationTypeId = "AAA-01,AAA-02" };
      s.ResponseExamples = new Dictionary<int, object> { { 204, new object() } };
    });
  }

  public override async Task HandleAsync(
    DeleteVerificationTypeRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationTypeId))
    {
      AddError(request => request.VerificationTypeId, "The verification type id of the record to be deleted is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new DeleteModelCommand<VerificationType>(CreateEndPointUser.GetEndPointUser(User), request.VerificationTypeId ?? "");
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
