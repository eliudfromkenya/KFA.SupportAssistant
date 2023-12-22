using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class DeleteQRCodesRequestValidator : Validator<DeleteQRCodesRequestRequest>
{
  public DeleteQRCodesRequestValidator()
  {
    RuleFor(x => x.QRCodeRequestID)
      .NotEmpty()
      .WithMessage("The qr code request id to be deleted is required please.")
      .MinimumLength(2)
      .MaximumLength(30);
  }
}
