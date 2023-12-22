using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

/// <summary>
/// Get a user login by login id.
/// </summary>
/// <remarks>
/// Takes login id and returns a matching user login record.
/// </remarks>
public class GetById(IMediator mediator, IEndPointManager endPointManager) : Endpoint<GetUserLoginByIdRequest, UserLoginRecord>
{
  private const string EndPointId = "ENP-244";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(GetUserLoginByIdRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get User Login End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Gets user login by specified login id";
      s.Description = "This endpoint is used to retrieve user login with the provided login id";
      s.ExampleRequest = new GetUserLoginByIdRequest { LoginId = "login id to retrieve" };
      s.ResponseExamples[200] = new UserLoginRecord(string.Empty, DateTime.Now, "1000", "Narration", DateTime.Now, string.Empty, DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(GetUserLoginByIdRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LoginId))
    {
      AddError(request => request.LoginId, "The login id of the record to be retrieved is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    var command = new GetModelQuery<UserLoginDTO, UserLogin>(CreateEndPointUser.GetEndPointUser(User), request.LoginId ?? "");
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
      Response = new UserLoginRecord(obj.DeviceId, obj.FromDate, obj.Id, obj.Narration, obj.UptoDate, obj.UserId, obj.DateInserted___, obj.DateUpdated___);
      return;
    }
  }
}
