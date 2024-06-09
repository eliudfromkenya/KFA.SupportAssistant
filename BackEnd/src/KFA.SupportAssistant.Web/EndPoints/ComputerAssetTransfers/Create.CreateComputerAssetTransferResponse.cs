namespace KFA.SupportAssistant.Web.EndPoints.ComputerAssetTransfers;

public readonly struct CreateComputerAssetTransferResponse(string? assetDetailID, string? costCentreCode, string? narration, string? payrollNumber, string? responsibleUser, string? status, global::System.DateTime? transferDate, string? transferID, string? transferReasons, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AssetDetailID { get; } = assetDetailID;
  public string? CostCentreCode { get; } = costCentreCode;
  public string? Narration { get; } = narration;
  public string? PayrollNumber { get; } = payrollNumber;
  public string? ResponsibleUser { get; } = responsibleUser;
  public string? Status { get; } = status;
  public global::System.DateTime? TransferDate { get; } = transferDate;
  public string? TransferID { get; } = transferID;
  public string? TransferReasons { get; } = transferReasons;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
