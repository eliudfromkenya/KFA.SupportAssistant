using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core.Models;
[Table("tbl_issues_attachments")]
public sealed record class IssuesAttachment : BaseModel
{
    public override string? ___tableName___ { get; protected set; } = "tbl_issues_attachments";
    // [Required]
    [Column("attachment_id")]
    public override string? Id { get; set; }

    [MaxLength(255, ErrorMessage = "Please attachment type must be 255 characters or less")]
    [Column("attachment_type")]
    public string? AttachmentType { get; init; }

    [Column("data")]
    public byte[]? Data { get; init; }

    [MaxLength(255, ErrorMessage = "Please description must be 255 characters or less")]
    [Column("description")]
    public string? Description { get; init; }

    [MaxLength(255, ErrorMessage = "Please file must be 255 characters or less")]
    [Column("file")]
    public string? File { get; init; }

    [Column("issue_id")]
    public string? IssueID { get; init; }

    [ForeignKey(nameof(IssueID))]
    public ProjectIssue? Issue { get; set; }
    [NotMapped]
    public string? Issue_Caption { get; set; }

    [MaxLength(500, ErrorMessage = "Please narration must be 500 characters or less")]
    [Column("narration")]
    public string? Narration { get; init; }

    [Column("time")]
    public global::System.DateTime Time { get; init; }
    public override object ToBaseDTO()
    {
        return (IssuesAttachmentDTO)this;
    }
}
