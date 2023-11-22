using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PPMS.Console.Models {
    public class TableRelation : BaseModel {
        public TableRelation () { }

        public TableRelation (string foreignKeyId, string masterColumnId, string foreignColumnId) {
            Id = foreignKeyId;
            MasterColumnId = masterColumnId;
            ForeignColumnId = foreignColumnId;
        }

        public override string Id { get; set; }
        public string MasterColumnId { get; set; }
        public string ForeignColumnId { get; set; }

        [ForeignKey ("MasterColumnId")]
        public TableColumn MasterColumn { get; set; }

        [ForeignKey ("ForeignColumnId")]
        public TableColumn ForeignColumn { get; set; }
    }
}