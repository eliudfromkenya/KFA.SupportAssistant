using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.LedgerAccounts;

/// <summary>
/// Create a new LedgerAccount
/// </summary>
/// <remarks>
/// Creates a new ledger account given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateLedgerAccountRequest, CreateLedgerAccountResponse>
{
  private const string EndPointId = "ENP-1J1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateLedgerAccountRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Ledger Account End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new ledger account";
      s.Description = "This endpoint is used to create a new  ledger account. Here details of ledger account to be created is provided";
      s.ExampleRequest = new CreateLedgerAccountRequest { CostCentreCode = "Cost Centre Code", Description = "Description", GroupName = "Group Name", IncreaseWithDebit = true, LedgerAccountCode = "Ledger Account Code", LedgerAccountId = "1000", MainGroup = "Main Group", Narration = "Narration" };
      s.ResponseExamples[200] = new CreateLedgerAccountResponse("Cost Centre Code", "Description", "Group Name", true, "Ledger Account Code", "1000", "Main Group", "Narration", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateLedgerAccountRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<LedgerAccountDTO>();
    requestDTO.Id = request.LedgerAccountId;

    var result = await mediator.Send(new CreateModelCommand<LedgerAccountDTO, LedgerAccount>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is LedgerAccountDTO obj)
      {
        Response = new CreateLedgerAccountResponse(obj.CostCentreCode, obj.Description, obj.GroupName, obj.IncreaseWithDebit, obj.LedgerAccountCode, obj.Id, obj.MainGroup, obj.Narration, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
