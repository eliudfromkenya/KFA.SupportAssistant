﻿using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.Verifications;

/// <summary>
/// List all verification.
/// </summary>
/// <remarks>
/// Dynamically Get all verification as specified - returns a dynamic list of the verification.
/// </remarks>
public class DynamicGet(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, string>
{
  private const string EndPointId = "ENP-2A3";
  public const string Route = "/verifications/dynamically";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Get Verifications Dynamically End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves dynamically of verifications as specified";
      s.Description = "Dynamically returns all verifications as specified, i.e filter to specify which records or rows to return, selector to specify which properties or columns to return, order to specify order criteria. It returns a JSON result in form of a string.";
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new (Id, Narration)", Parameters = ["SVR", "SVR-01"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new DynamicsListModelsQuery<VerificationDTO, Verification>(CreateEndPointUser.GetEndPointUser(User), request ?? new ListParam { });
    var result = await mediator.Send(command, cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = result.Value;
    }
  }
}
