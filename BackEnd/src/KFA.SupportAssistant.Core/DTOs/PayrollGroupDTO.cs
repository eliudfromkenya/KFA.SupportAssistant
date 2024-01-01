using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Core.DTOs;
public record class PayrollGroupDTO : BaseDTO<PayrollGroup>
{
  public string? GroupName { get; set; }
  public string? Narration { get; set; }
  public override PayrollGroup? ToModel()
  {
    return (PayrollGroup)this;
  }
  public static implicit operator PayrollGroupDTO(PayrollGroup obj)
  {
    return new PayrollGroupDTO
    {
      GroupName = obj.GroupName,
      Narration = obj.Narration,
      Id = obj.Id,
      DateInserted___ = obj.___DateInserted___?.ToDateTime(),
      DateUpdated___ = obj.___DateUpdated___?.ToDateTime()
    };
  }
  public static implicit operator PayrollGroup(PayrollGroupDTO obj)
  {
    return new PayrollGroup
    {
      GroupName = obj.GroupName ?? string.Empty,
      Narration = obj.Narration ?? string.Empty,
      Id = obj.Id ?? string.Empty,
      ___DateInserted___ = obj.DateInserted___.FromDateTime(),
      ___DateUpdated___ = obj.DateUpdated___.FromDateTime()
    };
  }
}
