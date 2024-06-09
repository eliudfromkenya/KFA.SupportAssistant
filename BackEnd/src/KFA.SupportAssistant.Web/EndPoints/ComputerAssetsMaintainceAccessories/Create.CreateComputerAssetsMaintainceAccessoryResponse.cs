namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetsMaintainceAccessories;

public readonly struct CreateComputerAssetsMaintainceAccessoryResponse(string? accessoryGroupID, string? accessoryID, decimal? amount, string? assetDetailID, global::System.DateTime? dateOfAcquisition, string? description, string? invoiceNumber, string? maintainceID, string? narration, string? quotationNumber, decimal? vATAmount, string? vendorCode, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AccessoryGroupID { get; } = accessoryGroupID;
  public string? AccessoryID { get; } = accessoryID;
  public decimal? Amount { get; } = amount;
  public string? AssetDetailID { get; } = assetDetailID;
  public global::System.DateTime? DateOfAcquisition { get; } = dateOfAcquisition;
  public string? Description { get; } = description;
  public string? InvoiceNumber { get; } = invoiceNumber;
  public string? MaintainceID { get; } = maintainceID;
  public string? Narration { get; } = narration;
  public string? QuotationNumber { get; } = quotationNumber;
  public decimal? VATAmount { get; } = vATAmount;
  public string? VendorCode { get; } = vendorCode;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
