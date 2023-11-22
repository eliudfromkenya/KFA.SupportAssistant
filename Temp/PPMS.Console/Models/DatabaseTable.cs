using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PPMS.Console.Models
{
    public class DatabaseTable : BaseModel
    {
        public DatabaseTable() { }

        public DatabaseTable(string name, string description, string tableId)
        {
            Name = Functions.StrimLineObjectName(name);
            OriginalName = name;
            Description = description;
            Id = tableId;
        }
        public string Name { get; set; }

        [NotMapped]
        public string StrimLinedName
        {
            get { return Functions.StrimLineObjectName(Name); }
        }

        static List<TableColumn> primaries = null;
        [NotMapped]
        public TableColumn PrimaryKey
        {
            get
            {
                if (primaries == null || !primaries.Any())
                {
                    using var tt = new Data.Context();
                    primaries = tt.PrimaryKeys.Select(x => x.Column).ToList();
                }
                return primaries?.FirstOrDefault(x => x.TableId == Id);
            }
        }

        public string OriginalName { get; set; }
        public string Description { get; set; }
        public override string Id { get; set; }
        public InitialData Data { get; set; }
        public ICollection<TableColumn> Columns { get; set; }
        public ICollection<TableRelation> Relations { get; set; }
    }
}