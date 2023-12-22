namespace KFA.SupportAssistant.Web.EndPoints.IssuesAttachments;

public readonly struct CreateIssuesAttachmentResponse(string? attachmentID, string? attachmentType, byte[]? data, string? description, string? file, string? issueID, string? narration, global::System.DateTime? time, DateTime? dateInserted___, DateTime? dateUpdated___)
{
  public string? AttachmentID { get; } = attachmentID;
  public string? AttachmentType { get; } = attachmentType;
  public byte[]? Data { get; } = data;
  public string? Description { get; } = description;
  public string? File { get; } = file;
  public string? IssueID { get; } = issueID;
  public string? Narration { get; } = narration;
  public global::System.DateTime? Time { get; } = time;
  public DateTime? DateInserted___ { get; } = dateInserted___;
  public DateTime? DateUpdated___ { get; } = dateUpdated___;
}
