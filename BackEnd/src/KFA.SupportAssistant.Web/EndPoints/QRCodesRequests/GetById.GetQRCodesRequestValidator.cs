using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class GetQRCodesRequestValidator : Validator<GetQRCodesRequestByIdRequest>
{
  public GetQRCodesRequestValidator()
  {
    RuleFor(x => x.QRCodeRequestID)
      .NotEmpty()
      .WithMessage("The qr code request id to be fetched is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
