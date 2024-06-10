namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetAcquisitions;

public readonly struct CreateComputerAssetAcquisitionResponse(string? acquisitionID, string? aquisitionType, string? assetDetailID, decimal? assetValue, global::System.DateTime? dateOfAcquisition, string? documentNo, string? narration, string? quotationNumber, string? receivedBy, decimal? value, decimal? vATAmount, string? vendorCode, global::System.DateTime? warantyEndDate, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AcquisitionID { get; } = acquisitionID;
  public string? AquisitionType { get; } = aquisitionType;
  public string? AssetDetailID { get; } = assetDetailID;
  public decimal? AssetValue { get; } = assetValue;
  public global::System.DateTime? DateOfAcquisition { get; } = dateOfAcquisition;
  public string? DocumentNo { get; } = documentNo;
  public string? Narration { get; } = narration;
  public string? QuotationNumber { get; } = quotationNumber;
  public string? ReceivedBy { get; } = receivedBy;
  public decimal? Value { get; } = value;
  public decimal? VATAmount { get; } = vATAmount;
  public string? VendorCode { get; } = vendorCode;
  public global::System.DateTime? WarantyEndDate { get; } = warantyEndDate;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
