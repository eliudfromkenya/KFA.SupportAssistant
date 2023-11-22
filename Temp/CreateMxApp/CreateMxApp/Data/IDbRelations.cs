namespace CreateMxApp.Data
{
  public interface IDbRelation
  {
    string? ForeignKeyId { get; set; }
    string? MasterTableName { get; set; }
    string? ConstraintName { get; set; }
    string? MasterColumnName { get; set; }
    string? ForeignTableName { get; set; }
    string? ForeignColumnName { get; set; }
  }
}
