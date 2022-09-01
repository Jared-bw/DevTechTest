using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DevTechTest.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public JobStatus Status { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required]
        public int ClientId { get; set; }

    }

    public enum JobStatus
    {
        SCHEDULED,
        ACTIVE,
        INVOICING,
        TO_PRICED,
        COMPLETED
    }
}
