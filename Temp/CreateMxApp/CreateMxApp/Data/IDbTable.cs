namespace CreateMxApp.Data;

public interface IDbTable
{
  string? Name { get; set; }
  string? OriginalName { get; set; }
  string? Description { get; set; }
  string? TableId { get; set; }
}
