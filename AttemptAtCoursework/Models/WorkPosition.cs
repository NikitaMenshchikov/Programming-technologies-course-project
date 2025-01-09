using System.ComponentModel.DataAnnotations;
namespace AttemptAtCoursework.Models
{
    public enum StatusForWorkPosition {
        [Display(Name = "Активное")] Active = 1, [Display(Name = "Неактивное")] Inactive = 2, [Display(Name = "Рассматриваемое менеджером")]
         ConsideredByTheManager = 3
    }
    public class WorkPosition
    {
        [Display(Name = "Номер")]
        public uint Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        [StringLength(50, ErrorMessage = "Максимальное количество символов для названия должности — 50.")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Статус")]
        public StatusForWorkPosition Status { get; set; }


    }
}
