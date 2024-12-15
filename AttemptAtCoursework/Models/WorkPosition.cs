using System.ComponentModel.DataAnnotations;
namespace AttemptAtCoursework.Models
{
    public enum StatusForWorkPosition {Active = 1, Inactive = 2 }
    public class WorkPosition
    {
        public uint Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The maximum number of symbols for work position is 50.")]
        public string? Name { get; set; }

        [Required]
        public StatusForWorkPosition Status { get; set; }


    }
}
