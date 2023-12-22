using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.SystemUsers;

/// <summary>
/// Get a system user by user id.
/// </summary>
/// <remarks>
/// Takes user id and returns a matching system user record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetSystemUserByIdRequest, SystemUserRecord>
{
  private const string EndPointId = "ENP-214";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetSystemUserByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get System User End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets system user by specified user id";
      s.Description = "This endpoint is used to retrieve system user with the provided user id";
      s.ExampleRequest = new GetSystemUserByIdRequest { UserId = "user id to retrieve" };
      s.ResponseExamples[200] = new SystemUserRecord("Contact", "Email Address", DateTime.Now, true, DateTime.Now, "Name Of The User", "Narration", string.Empty, "1000", "Username", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetSystemUserByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.UserId))
    {
      AddError(request => request.UserId, "The user id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<SystemUserDTO, SystemUser>(CreateEndPointUser.GetEndPointUser(User), request.UserId ?? "");
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
      Response = new SystemUserRecord(obj.Contact, obj.EmailAddress, obj.ExpirationDate, obj.IsActive, obj.MaturityDate, obj.NameOfTheUser, obj.Narration, obj.RoleId, obj.Id, obj.Username, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
