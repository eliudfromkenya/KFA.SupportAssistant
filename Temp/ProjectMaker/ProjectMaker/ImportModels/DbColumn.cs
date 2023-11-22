#region

using Pilgrims.ProjectManagement.Contracts.DataImport;


#endregion

namespace Pilgrims.ProjectManagement.DataImport.Models
{
    internal class DbColumn : IDbColumn
    {
        //public ITableColumn TableColumn { get; set; }
        public string ColumnId { get; set; }

        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string OriginalColumnName { get; set; }
        public bool IsUnique { get; set; }
        public string Description { get; set; }
        public byte Position { get; set; }
        public bool HasDefault { get; set; }
        public string Default { get; set; }
        public int ColumnFlags { get; set; }
        public bool IsNullable { get; set; }
        public int DataType { get; set; }
        public int Length { get; set; }
    }
}