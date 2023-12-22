using FluentValidation;

namespace KFA.SupportAssistant.Web.EndPoints.QRCodesRequests;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class PatchQRCodesRequestValidator : Validator<PatchQRCodesRequestRequest>
{
  public PatchQRCodesRequestValidator()
  {
    RuleFor(x => x.QRCodeRequestID)
     .NotEmpty()
     .WithMessage("The qr code request id of the record to be updated is required")
     .MinimumLength(2)
     .MaximumLength(30);

    RuleFor(x => x.Content)
    .NotEmpty()
    .WithMessage("Body or content to update is required.");
  }
}
