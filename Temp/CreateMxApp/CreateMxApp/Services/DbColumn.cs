using CreateMxApp.Data;

namespace CreateMxApp.Services
{
  internal class DbColumn : IDbColumn
  {
    public string? ColumnId { get; set; }
    public string? TableName { get; set; }
    public string? ColumnName { get; set; }
    public string? OriginalColumnName { get; set; }
    public bool IsUnique { get; set; }
    public string? Description { get; set; }
    public byte Position { get; set; }
    public bool HasDefault { get; set; }
    public string? Default { get; set; }
    public int ColumnFlags { get; set; }
    public bool IsNullable { get; set; }
    public (string? Id, TypeCode TypeCode) DataType { get; set; }
    public int Length { get; set; }
    public int Precision { get; set; }
  }
}
