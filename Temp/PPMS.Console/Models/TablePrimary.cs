using System.Linq;
namespace PPMS.Console.Models {
    public class TablePrimary : BaseModel {
        public TablePrimary () { }
        //public TablePrimary (string primaryKeyId, string columnName, string tableName) {
        //    try {
        //        Id = primaryKeyId;
        //        ColumnId = Data.OldData.DefaultColumns.First (x => x.ColumnName.MakeName () == columnName.MakeName () && x.TableId.MakeName () == tableName.MakeName ()).Id;
        //    } catch { }
        //}

        public TablePrimary (string primaryKeyId, string columnId) {
            Id = primaryKeyId;
            ColumnId = columnId;
        }
        public override string Id { get; set; }
        public string ColumnId { get; set; }
        public TableColumn Column { get; set; }
    }
}