namespace KFA.SupportAssistant.Web.EndPoints.Vendors;

public class UpdateVendorResponse
{
  public UpdateVendorResponse(VendorRecord vendor)
  {
    Vendor = vendor;
  }

  public VendorRecord Vendor { get; set; }
}
