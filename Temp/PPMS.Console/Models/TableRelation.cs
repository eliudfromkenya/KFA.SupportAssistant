using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PPMS.Console.Models
{
    public class TableRelation : BaseModel
    {
        public TableRelation() { }

        public TableRelation(string foreignKeyId, string masterColumnId, string foreignColumnId)
        {
            Id = foreignKeyId;
            MasterColumnId = masterColumnId;
            ForeignColumnId = foreignColumnId;
        }

        //public TableRelation (string foreignKeyId, string masterTableName, string masterColumnName, string foreignTableName, string foreignColumnName) {
        //    Id = foreignKeyId;
        //    try {
        //        MasterColumnId = Data.OldData.DefaultColumns.First (x => x.ColumnName.MakeName () == masterColumnName.MakeName () && x.TableId.MakeName () == masterTableName.MakeName ()).Id;
        //        ForeignColumnId = Data.OldData.DefaultColumns.First (x => x.ColumnName.MakeName () == foreignColumnName.MakeName () && x.TableId.MakeName () == foreignTableName.MakeName ()).Id;
        //    } catch (System.Exception) {
        //        System.Console.WriteLine (foreignKeyId, masterTableName, masterColumnName);
        //    }

        //}

        public override string Id { get; set; }
        public string MasterColumnId { get; set; }
        public string ForeignColumnId { get; set; }

        [ForeignKey("MasterColumnId")]
        public TableColumn MasterColumn { get; set; }

        [ForeignKey("ForeignColumnId")]
        public TableColumn ForeignColumn { get; set; }
    }
}