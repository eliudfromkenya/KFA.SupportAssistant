using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPMS.Console.Models {
    public class DatabaseTable : BaseModel {
        public DatabaseTable () { }

        public DatabaseTable (string name, string description, string tableId) {
            Name = Functions.StrimLineObjectName (name);
            OriginalName = name;
            Description = description;
            Id = tableId;
        }
        public string Name { get; set; }

        [NotMapped]
        public string StrimLinedName {
            get { return Functions.StrimLineObjectName (Name); }
        }

        public string OriginalName { get; set; }
        public string Description { get; set; }
        public override string Id { get; set; }
       //q public InitialData Data { get; set; }
        public ICollection<TableColumn> Columns { get; set; }
        public ICollection<TableRelation> Relations { get; set; }
    }
}