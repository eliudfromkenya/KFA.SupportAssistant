using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class EmployeeDetailDTO : BaseDTO<EmployeeDetail>
{
  public decimal AmountDue { get; set; }
  public string? CostCentreCode { get; set; }
  public global::System.DateTime? Date { get; set; }
  public string? Email { get; set; }
  public string? FullName { get; set; }
  public string? Gender { get; set; }
  public string? GroupNumber { get; set; }
  public string? IdNumber { get; set; }
  public string? Narration { get; set; }
  public string? PayrollNumber { get; set; }
  public string? PhoneNumber { get; set; }
  public global::System.DateTime? RejoinDate { get; set; }
  public decimal? RetireeAmount { get; set; }
  public decimal? RetrenchmentAmount { get; set; }
  public global::System.DateTime? RetrenchmentDate { get; set; }
  public string? Status { get; set; }
  public string? Remarks { get; set; }
  public string? PayrollGroupID { get; set; }
  public string? Classification { get; set; }
  public override EmployeeDetail? ToModel()
  {
    return (EmployeeDetail)this;
  }
  public static implicit operator EmployeeDetailDTO(EmployeeDetail obj)
  {
    return new EmployeeDetailDTO
    {
      AmountDue = ToDecimal(obj.AmountDue) ?? 0,
      Classification = obj.Classification,
      CostCentreCode = obj.CostCentreCode,
      Date = ToDate(obj.Date),
      Email = obj.Email,
      FullName = obj.FullName,
      Gender = obj.Gender,
      GroupNumber = obj.GroupNumber,
      IdNumber = obj.IdNumber,
      Narration = obj.Narration,
      PayrollGroupID = obj.PayrollGroupID,
      PayrollNumber = obj.PayrollNumber,
      PhoneNumber = obj.PhoneNumber,
      RejoinDate = ToDate(obj.RejoinDate),
      Remarks = obj.Remarks,
      RetireeAmount = ToDecimal(obj.RetireeAmount),
      RetrenchmentAmount = ToDecimal(obj.RetrenchmentAmount),
      RetrenchmentDate = ToDate(obj.RetrenchmentDate),
      Status = obj.Status,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator EmployeeDetail(EmployeeDetailDTO obj)
  {
    return new EmployeeDetail
    {
      AmountDue = obj.AmountDue.ToString(),
      Classification = obj?.Classification ?? string.Empty,
      CostCentreCode = obj?.CostCentreCode,
      Date = obj?.Date?.ToString() ?? string.Empty,
      Email = obj?.Email ?? string.Empty,
      FullName = obj?.FullName ?? string.Empty,
      Gender = obj?.Gender ?? string.Empty,
      GroupNumber = obj?.GroupNumber ?? string.Empty,
      IdNumber = obj?.IdNumber ?? string.Empty,
      Narration = obj?.Narration ?? string.Empty,
      PayrollGroupID = obj?.PayrollGroupID,
      PayrollNumber = obj?.PayrollNumber ?? string.Empty,
      PhoneNumber = obj?.PhoneNumber ?? string.Empty,
      RejoinDate = obj?.RejoinDate?.ToString() ?? string.Empty,
      Remarks = obj?.Remarks ?? string.Empty,
      RetireeAmount = obj?.RetireeAmount.ToString() ?? string.Empty,
      RetrenchmentAmount = obj?.RetrenchmentAmount.ToString() ?? string.Empty,
      RetrenchmentDate = obj?.RetrenchmentDate.ToString() ?? string.Empty,
      Status = obj?.Status ?? string.Empty,
      Id = obj?.Id,
      ___DateInserted___ = obj?.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj?.DateUpdated___.FromDateTime()
    };
  }
}
