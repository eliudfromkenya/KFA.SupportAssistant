using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Classes;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Patch;
using KFA.SupportAssistant.Web.Services;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

public class Patch(IMediator mediator, IEndPointManager endPointManager) : Endpoint<PatchDuesPaymentDetailRequest, DuesPaymentDetailRecord>
{
  private const string EndPointId = "ENP-1A6";

  public override void Configure()
  {
    Patch(CoreFunctions.GetURL(PatchDuesPaymentDetailRequest.Route));
    //RequestBinder(new PatchBinder<DuesPaymentDetailDTO, DuesPaymentDetail, PatchDuesPaymentDetailRequest>());
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Partial Update Dues Payment Detail End Point"));
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Update partially a dues payment detail";
      s.Description = "Used to update part of an existing dues payment detail. A valid existing dues payment detail is required.";
      s.ResponseExamples[200] = new DuesPaymentDetailRecord(0, DateTime.Now, "Document No", string.Empty, true, "Narration", 0, "1000", "Payment Type", "Processed By", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(PatchDuesPaymentDetailRequest request, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(request.PaymentID))
    {
      AddError(request => request.PaymentID, "The dues payment detail of the record to be updated is required please");
      await SendErrorsAsync(statusCode: 400, cancellation: cancellationToken);
      return;
    }

    DuesPaymentDetailDTO patchFunc(DuesPaymentDetailDTO tt) => AsyncUtil.RunSync(() => PatchUpdater.Patch<DuesPaymentDetailDTO, DuesPaymentDetail>(() => request.PatchDocument, HttpContext, request.Content, tt, cancellationToken));
    var result = await mediator.Send(new PatchModelCommand<DuesPaymentDetailDTO, DuesPaymentDetail>(CreateEndPointUser.GetEndPointUser(User), request.PaymentID ?? "", patchFunc), cancellationToken);

    if (result.Status == ResultStatus.NotFound)
      AddError("Can not find the dues payment detail to update");

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    var obj = result.Value;
    if (result.IsSuccess)
    {
      Response = new DuesPaymentDetailRecord(obj.Amount, obj.Date, obj.DocumentNo, obj.EmployeeID, obj.IsFinalPayment, obj.Narration, obj.OpeningBalance, obj.Id, obj.PaymentType, obj.ProcessedBy, obj.DateInserted___, obj.DateUpdated___);
    }
  }
}
