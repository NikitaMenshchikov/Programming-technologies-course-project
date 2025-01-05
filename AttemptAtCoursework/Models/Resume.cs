using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AttemptAtCoursework.Models
{
    public enum Experience
    {
        [Display(Name = "Нет опыта")]
        NoExperience = 1,
        [Display(Name = "От одного до трёх лет")]
        FromOneYearToThreeYears = 2,
        [Display(Name = "От трёх до шести лет")]
        FromThreeToSixYears = 3,
        [Display(Name = "Больше шести лет")]
        MoreThanSixYears = 4
    }
    public enum AdvertisedEmploymentType
    {
        [Display(Name = "Полная занятость")]
        FullEmployment = 1,
        [Display(Name = "Частичная занятость")]
        PartTime = 2
    }
    public enum StatusforResume
    {
        [Display(Name = "Рассматриваемое менеджером")]
        ConsideredByTheManager = 1,
        [Display(Name = "Активное")] Active = 2,
        [Display(Name = "Неактивное")] Inactive = 3
    }
    public class Resume
    {
        public uint Id { get; set; }

        [Required]
        public uint WorkPositionId { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Максимальное количество символов для резюме — 1000.")]
        public string? Content { get; set; }

        [Required]
        public Experience Experience { get; set; }

        [Required]
        public AdvertisedEmploymentType AdvertisedEmploymentType { get; set; }

        public uint VacancyId { get; set; }

        [EmailAddress]
        public string? ApplicantMail { get; set; }

        public string? RecommendedToEmployerId { get; set; }

        [Required]
        public StatusforResume Status { get; set; }

        public string GetDisplayName(Enum val)
        {
            return val.GetType()
                      .GetMember(val.ToString())
                      .FirstOrDefault()
                      ?.GetCustomAttribute<DisplayAttribute>(false)
                      ?.Name
                      ?? val.ToString();
        }
    }
}
