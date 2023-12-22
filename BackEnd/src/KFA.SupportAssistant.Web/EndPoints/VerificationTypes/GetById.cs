using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.VerificationTypes;

/// <summary>
/// Get a verification type by verification type id.
/// </summary>
/// <remarks>
/// Takes verification type id and returns a matching verification type record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetVerificationTypeByIdRequest, VerificationTypeRecord>
{
  private const string EndPointId = "ENP-294";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetVerificationTypeByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Verification Type End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets verification type by specified verification type id";
      s.Description = "This endpoint is used to retrieve verification type with the provided verification type id";
      s.ExampleRequest = new GetVerificationTypeByIdRequest { VerificationTypeId = "verification type id to retrieve" };
      s.ResponseExamples[200] = new VerificationTypeRecord("Category", "Narration", "1000", "Verification Type Name", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetVerificationTypeByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.VerificationTypeId))
    {
      AddError(request => request.VerificationTypeId, "The verification type id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<VerificationTypeDTO, VerificationType>(CreateEndPointUser.GetEndPointUser(User), request.VerificationTypeId ?? "");
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
      Response = new VerificationTypeRecord(obj.Category, obj.Narration, obj.Id, obj.VerificationTypeName, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
