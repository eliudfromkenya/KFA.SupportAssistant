using System;
using System.Collections.Generic;
using System.Text;

namespace PPMS.Console.Data
{
    public struct XTable
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Description { get; set; }
        public string TableId { get; set; }
    }

    public struct XColumn
    {
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

    public struct XPrimary
    {
        public string PrimaryKeyId { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
    }

    public struct XRelation
    {
        public string ForeignKeyId { get; set; }
        public string MasterTableName { get; set; }
        public string MasterColumnName { get; set; }
        public string ForeignTableName { get; set; }
        public string ForeignColumnName { get; set; }
    }
}
