using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PPMS.Console.Models
{
    public class TableColumn : BaseModel
    {
        public override string Id { get; set; }
        public string TableId { get; set; }
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
        [ForeignKey("TableId")]
        public DatabaseTable Table { get; set; }


        private static List<TableColumn> primaries = null;
        [NotMapped]
        public bool IsPrimary
        {
            get
            {
                if (primaries == null || !primaries.Any())
                {
                    using var tt = new Data.Context();
                    primaries = tt.PrimaryKeys.Select(x => x.Column).ToList();
                }
                return true == primaries?.Any(x => x.Id == Id);
            }
        }
        [NotMapped]
        public string Type
        {
            get { return Functions.GetTypes(DataType); }
        }


        public TableColumn() { }

        public TableColumn(string columnId, string tableId, string columnName, bool isUnique, string description,
            byte position, bool hasDefault, string @default, int columnFlags, bool isNullable, int dataType, int length)
        {
            Id = columnId;
            TableId = tableId;
            ColumnName = Functions.StrimLineObjectName(columnName);
            OriginalColumnName = columnName;
            IsUnique = isUnique;
            Description = description;
            Position = position;
            HasDefault = hasDefault;
            Default = @default;
            ColumnFlags = columnFlags;
            IsNullable = isNullable;
            DataType = dataType;
            Length = length;
        }

        [NotMapped]
        public string StrimLinedName
        {
            get { return Functions.StrimLineObjectName(ColumnName); }
        }
    }
}