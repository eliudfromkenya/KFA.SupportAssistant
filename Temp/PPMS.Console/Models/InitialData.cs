using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PPMS.Console.Models {
    public class InitialData : BaseModel {
        public InitialData () { }
        public override string Id { get; set; }
        public string TableId { get; set; }
        public string Data { get; set; }
        public int RowCount { get; set; }
        public string Columns { get; set; }
        //public InitialData (string dataId, string tableId, string data, int rowCount, string columns = "") {
        //    Id = dataId;
        //    TableId = PPMS.Console.Data.OldData.DefaultTables.FirstOrDefault (x => x.Name.MakeName () == tableId.MakeName ())?.Id ?? tableId;
        //    Data = data;
        //    RowCount = rowCount;
        //    Columns = columns;
        //}
        //[NotMapped]
        public DatabaseTable Table { get; set; }
    }
}