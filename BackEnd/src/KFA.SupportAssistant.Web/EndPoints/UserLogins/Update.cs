using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.UserLogins;

/// <summary>
/// Update an existing user login.
/// </summary>
/// <remarks>
/// Update an existing user login by providing a fully defined replacement set of values.
/// See: https://stackoverflow.com/questions/60761955/rest-update-best-practice-put-collection-id-without-id-in-body-vs-put-collecti
/// </remarks>
public class Update(IMediator mediator, IEndPointManager endPointManager) : Endpoint<UpdateUserLoginRequest, UpdateUserLoginResponse>
{
  private const string EndPointId = "ENP-247";

  public override void Configure()
  {
    Put(CoreFunctions.GetURL(UpdateUserLoginRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Update User Login End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update a full User Login";
      s.Description = "This endpoint is used to update  user login, making a full replacement of user login with a specifed valuse. A valid user login is required.";
      s.ExampleRequest = new UpdateUserLoginRequest { DeviceId = string.Empty, FromDate = DateTime.Now, LoginId = "1000", Narration = "Narration", UptoDate = DateTime.Now, UserId = string.Empty };
      s.ResponseExamples[200] = new UpdateUserLoginResponse(new UserLoginRecord(string.Empty, DateTime.Now, "1000", "Narration", DateTime.Now, string.Empty, DateTime.Now, DateTime.Now));
    });
  }

  public override async Task HandleAsync(
    UpdateUserLoginRequest request,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.LoginId))
    {
      AddError(request => request.LoginId, "The login id of the record to be updated is required please");

      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);

      return;
    }

    var command = new GetModelQuery<UserLoginDTO, UserLogin>(CreateEndPointUser.GetEndPointUser(User), request.LoginId ?? "");
    var resultObj = await mediator.Send(command, cancellationToken);

    if (resultObj.Errors.Any())
    {
      resultObj.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, resultObj.Status, resultObj.Errors, cancellationToken);
    }

    if (resultObj.Status == ResultStatus.NotFound)
    {
      AddError("Can not find the user login to update");
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    var value = request.Adapt(resultObj.Value);
    var result = await mediator.Send(new UpdateModelCommand<UserLoginDTO, UserLogin>(CreateEndPointUser.GetEndPointUser(User), request.LoginId ?? "", value!), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
    {
      await SendNotFoundAsync(cancellationToken);
      return;
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new UpdateUserLoginResponse(new UserLoginRecord(obj.DeviceId, obj.FromDate, obj.Id, obj.Narration, obj.UptoDate, obj.UserId, obj.DateInserted___, obj.DateUpdated___));
      return;
    }
  }
}
