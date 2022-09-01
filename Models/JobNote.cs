using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTechTest.Models
{
    public class JobNote
    {
        [Key]
        public int Id { get; set; }

        
        [Required]
        public int JobId { get; set; }


        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Required]
        public string Note { get; set; }
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
