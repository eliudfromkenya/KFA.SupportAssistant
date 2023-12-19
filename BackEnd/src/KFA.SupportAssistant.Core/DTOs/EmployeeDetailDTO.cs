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
  public override EmployeeDetail? ToModel()
  {
    return (EmployeeDetail)this;
  }
  public static implicit operator EmployeeDetailDTO(EmployeeDetail obj)
  {
    return new EmployeeDetailDTO
    {
      AmountDue = decimal.TryParse( obj.AmountDue, out decimal amt)?amt:0,
      CostCentreCode = obj.CostCentreCode,
      Date = DateTime.TryParse( obj.Date, out DateTime date)?date: null,
      Email = obj.Email,
      FullName = obj.FullName,
      Gender = obj.Gender,
      GroupNumber = obj.GroupNumber,
      IdNumber = obj.IdNumber,
      Narration = obj.Narration,
      PayrollNumber = obj.PayrollNumber,
      PhoneNumber = obj.PhoneNumber,
      RejoinDate = DateTime.TryParse(obj.RejoinDate, out DateTime date1) ? date1 : null,
      RetireeAmount = decimal.TryParse(obj.RetireeAmount, out decimal amt2) ? amt2 : null,
      RetrenchmentAmount = decimal.TryParse(obj.RetrenchmentAmount, out decimal amt3) ? amt3 : null,
      RetrenchmentDate = DateTime.TryParse(obj.RetrenchmentDate, out DateTime date2) ? date2 : null,
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
      CostCentreCode = obj.CostCentreCode,
      Date = obj.Date?.ToString(),
      Email = obj.Email,
      FullName = obj.FullName,
      Gender = obj.Gender,
      GroupNumber = obj.GroupNumber,
      IdNumber = obj.IdNumber,
      Narration = obj.Narration,
      PayrollNumber = obj.PayrollNumber,
      PhoneNumber = obj.PhoneNumber,
      RejoinDate = obj.RejoinDate?.ToString(),
      RetireeAmount = obj.RetireeAmount?.ToString(),
      RetrenchmentAmount = obj.RetrenchmentAmount?.ToString(),
      RetrenchmentDate = obj.RetrenchmentDate?.ToString(),
      Status = obj.Status,
      Id = obj.Id,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }

}
