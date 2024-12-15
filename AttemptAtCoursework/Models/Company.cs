using System.ComponentModel.DataAnnotations;

namespace AttemptAtCoursework.Models
{
    public enum StatusforCompany
    {
        [Display(Name = "Рассматриваемое менеджером")]
        ConsideredByTheManager = 1,
        [Display(Name = "Активное")] Active = 2,
        [Display(Name = "Неактивное")] Inactive = 3
    }

    public enum TypeOfProduction
    {
        [Display(Name = "Строительство")]
        Construction = 1,
        [Display(Name = "Медицина")] Medicine = 2,
        [Display(Name = "IT")] IT = 3,
        [Display(Name = "Экономика")] Economy = 4,
        [Display(Name = "Туризм")] Tourism = 5,
    }

    public enum CityEnum
    {
        [Display(Name = "Владимир")]
        Vladimir = 1,
        [Display(Name = "Москва")] Moscow = 2,
        [Display(Name = "Нижний Новогород")] NizhnyNovgorod = 3,
        [Display(Name = "Санкт-Петербург")] SaintPetersburg = 4,
        [Display(Name = "Казань")] Kazan = 5,
        [Display(Name = "Екатеринбург")] Yekaterinburg = 6,
        [Display(Name = "Краснодар")] Krasnodar = 7,
    }

    public class Company
    {
        public uint Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Максимальное количество символов для названия компании — 50.")]
        public string? Name { get; set; }

        [Required]
        public CityEnum City { get; set; }

        [Required]
        public TypeOfProduction TypeOfProduction { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Максимальное количество символов для описания компании — 1000.")]
        public string? Description { get; set; }

        [Required]
        public Status Status { get; set; }
    }
}
