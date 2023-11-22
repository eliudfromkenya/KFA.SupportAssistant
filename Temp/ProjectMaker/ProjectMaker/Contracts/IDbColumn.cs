namespace Pilgrims.ProjectManagement.Contracts.DataImport
{
    public interface IDbColumn
    {
        string ColumnId { get; set; }

        string TableName { get; set; }
        string ColumnName { get; set; }
        string OriginalColumnName { get; set; }
        bool IsUnique { get; set; }
        string Description { get; set; }
        byte Position { get; set; }
        bool HasDefault { get; set; }
        string Default { get; set; }
        int ColumnFlags { get; set; }
        bool IsNullable { get; set; }
        int DataType { get; set; }
        int Length { get; set; }
    }
}