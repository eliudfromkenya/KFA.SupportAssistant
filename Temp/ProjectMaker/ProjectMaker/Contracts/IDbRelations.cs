namespace Pilgrims.ProjectManagement.Contracts.DataImport
{
    public interface IDbRelations
    {
        string ForeignKeyId { get; set; }
        string MasterTableName { get; set; }
        string MasterColumnName { get; set; }
        string ForeignTableName { get; set; }
        string ForeignColumnName { get; set; }
    }
}