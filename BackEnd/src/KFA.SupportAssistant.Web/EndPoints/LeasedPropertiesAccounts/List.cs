using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.LeasedPropertiesAccounts;

/// <summary>
/// List all leased properties accounts by specified conditions
/// </summary>
/// <remarks>
/// List all leased properties accounts - returns a LeasedPropertiesAccountListResponse containing the leased properties accounts.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, LeasedPropertiesAccountListResponse>
{
  private const string EndPointId = "ENP-1I5";
  public const string Route = "/leased_properties_accounts";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Leased Properties Accounts List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of leased properties accounts as specified";
      s.Description = "Returns all leased properties accounts as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new LeasedPropertiesAccountListResponse { LeasedPropertiesAccounts = [new LeasedPropertiesAccountRecord(0, "Cost Centre Code", 0, "Landlord Address", DateTime.Now, DateTime.Now, "1000", "Ledger Account Code", "Narration", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<LeasedPropertiesAccountDTO, LeasedPropertiesAccount>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<LeasedPropertiesAccountDTO>>.Success(ans.Select(v => (LeasedPropertiesAccountDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new LeasedPropertiesAccountListResponse
      {
        LeasedPropertiesAccounts = result.Value.Select(obj => new LeasedPropertiesAccountRecord(obj.CommencementRent, obj.CostCentreCode, obj.CurrentRent, obj.LandlordAddress, obj.LastReviewDate, obj.LeasedOn, obj.Id, obj.LedgerAccountCode, obj.Narration, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
