using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmIAuthorised.DataAccessLayer.Entity
{
    [Table("application", Schema = "public")]
    public class Application
    {
        [Key]
        [Column("application_id")]
        public long ApplicationId { get; set; }

        [Column("application_number")]
        public string ApplicationNumber { get; set; } = string.Empty;

        [Column("application_name")]
        public string ApplicationName { get; set; } = string.Empty;

        [Column("application_status")]
        public int ApplicationStatus { get; set; }

        [Column("created_by")]
        public long CreatedBy { get; set; }


        [ForeignKey(nameof(CreatedBy))]
        public virtual User? User { get; set; }
    }
}
