using System.Collections.Generic;
using System.Linq;

namespace PPMS.Console.Models {
  public class DataGroup : BaseModel {
    //public DataGroup (int groupId, string tableId, string strimLinedName, string groupName, string imagePath) {
    //  Id = groupId.ToString ();
    //  TableId = OldData.DefaultTables.FirstOrDefault (x => x.Name.MakeName () == tableId.MakeName ())?.Id ?? tableId;
    //  StrimLinedName = strimLinedName;
    //  GroupName = groupName;
    //  ImagePath = imagePath;
    //}
    public DataGroup () {

    }
    public override string Id { get; set; }
    public string TableId { get; set; }
    public string StrimLinedName { get; set; }
    public string GroupName { get; set; }
    public string ImagePath { get; set; }
    public DatabaseTable Tables { get; set; }
  }
}