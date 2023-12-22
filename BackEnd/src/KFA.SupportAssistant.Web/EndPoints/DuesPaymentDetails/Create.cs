using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.Web.Services;
using Mapster;
using MediatR;

namespace KFA.SupportAssistant.Web.EndPoints.DuesPaymentDetails;

/// <summary>
/// Create a new DuesPaymentDetail
/// </summary>
/// <remarks>
/// Creates a new dues payment detail given by given details.
/// </remarks>
public class Create(IMediator mediator, IEndPointManager endPointManager) : Endpoint<CreateDuesPaymentDetailRequest, CreateDuesPaymentDetailResponse>
{
  private const string EndPointId = "ENP-1A1";

  public override void Configure()
  {
    Post(CoreFunctions.GetURL(CreateDuesPaymentDetailRequest.Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);
    Description(x => x.WithName("Add Dues Payment Detail End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Used to create a new dues payment detail";
      s.Description = "This endpoint is used to create a new  dues payment detail. Here details of dues payment detail to be created is provided";
      s.ExampleRequest = new CreateDuesPaymentDetailRequest { Amount = 0, Date = DateTime.Now, DocumentNo = "Document No", EmployeeID = string.Empty, IsFinalPayment = true, Narration = "Narration", OpeningBalance = 0, PaymentID = "1000", PaymentType = "Payment Type", ProcessedBy = "Processed By" };
      s.ResponseExamples[200] = new CreateDuesPaymentDetailResponse(0, DateTime.Now, "Document No", string.Empty, true, "Narration", 0, "1000", "Payment Type", "Processed By", DateTime.Now, DateTime.Now);
    });
  }

  public override async Task HandleAsync(
    CreateDuesPaymentDetailRequest request,
    CancellationToken cancellationToken)
  {
    var requestDTO = request.Adapt<DuesPaymentDetailDTO>();
    requestDTO.Id = request.PaymentID;

    var result = await mediator.Send(new CreateModelCommand<DuesPaymentDetailDTO, DuesPaymentDetail>(CreateEndPointUser.GetEndPointUser(User), requestDTO), cancellationToken);

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      if (result?.Value?.FirstOrDefault() is DuesPaymentDetailDTO obj)
      {
        Response = new CreateDuesPaymentDetailResponse(obj.Amount, obj.Date, obj.DocumentNo, obj.EmployeeID, obj.IsFinalPayment, obj.Narration, obj.OpeningBalance, obj.Id, obj.PaymentType, obj.ProcessedBy, obj.DateInserted___, obj.DateUpdated___);
        return;
      }
    }
  }
}
