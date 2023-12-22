using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.PasswordSafes;

/// <summary>
/// Get a password safe by password id.
/// </summary>
/// <remarks>
/// Takes password id and returns a matching password safe record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetPasswordSafeByIdRequest, PasswordSafeRecord>
{
  private const string EndPointId = "ENP-1L4";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetPasswordSafeByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Password Safe End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets password safe by specified password id";
      s.Description = "This endpoint is used to retrieve password safe with the provided password id";
      s.ExampleRequest = new GetPasswordSafeByIdRequest { PasswordId = "password id to retrieve" };
      s.ResponseExamples[200] = new PasswordSafeRecord("Details", "Name", "Password", "1000", "Reminder", "Users Visible To", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetPasswordSafeByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.PasswordId))
    {
      AddError(request => request.PasswordId, "The password id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<PasswordSafeDTO, PasswordSafe>(CreateEndPointUser.GetEndPointUser(User), request.PasswordId ?? "");
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
      Response = new PasswordSafeRecord(obj.Details, obj.Name, obj.Password, obj.Id, obj.Reminder, obj.UsersVisibleTo, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
