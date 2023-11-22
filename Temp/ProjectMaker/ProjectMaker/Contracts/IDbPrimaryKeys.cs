namespace Pilgrims.ProjectManagement.Contracts.DataImport
{
    public interface IDbPrimaryKeys
    {
        string PrimaryKeyId { get; set; }
        string Name { get; set; }
        string TableName { get; set; }
    }
}