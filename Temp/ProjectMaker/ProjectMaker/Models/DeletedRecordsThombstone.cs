using System.ComponentModel.DataAnnotations.Schema;

namespace PPMS.Console.Models {
    [Table ("thombstone_deleted_records")]
    public class DeletedRecordsThombstone {
        [Column ("rec_id")]
        public string RecId { get; set; }

        [Column ("table_name")]
        public string TableName { get; set; }

        [Column ("dated_deleted")]
        public long DateDeleted { get; set; }

        [Column ("user_trail_id")]
        public string UserTrailId { get; set; }
    }
}