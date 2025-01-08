using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AttemptAtCoursework.Models
{
    public enum RequiredExperience {
        [Display(Name ="Нет опыта")]
        NoExperience = 1,
        [Display(Name = "От одного до трёх лет")]
        FromOneYearToThreeYears =2,
        [Display(Name = "От трёх до шести лет")]
        FromThreeToSixYears =3,
        [Display(Name = "Больше шести лет")]
        MoreThanSixYears =4 }
    public enum TypeOfEmployment {
        [Display(Name = "Полная занятость")]
        FullEmployment =1,
        [Display(Name = "Частичная занятость")] 
        PartTime =2,
        [Display(Name = "Стажировка")]
        Internship=3 }

    public enum Status { [Display(Name = "Рассматриваемое менеджером")]
        ConsideredByTheManager = 1, 
        [Display(Name = "Активное")] Active=2, 
        [Display(Name = "Неактивное")] Inactive=3 }
    public class Vacancy
    {
        [Display(Name = "Номер")]
        public uint Id { get; set; }

        [Required]
        [Range(1, UInt32.MaxValue, ErrorMessage = "WorkPositionId must be greater than zero and less than 4294967295.")]
        [Display(Name = "Номер должности")]
        public uint WorkPositionId { get; set; }

        [Required]
        [Display(Name = "Количество требуемых сотрудников")]
        [Range(1, UInt32.MaxValue, ErrorMessage = "Number of required applicants must be positive and less than 4294967295")]
        public uint NumberOfRequiredApplicants { get; set; }

        [Required]
        [Display(Name = "Количество нанятых сотрудников")]
        public uint NumberOfApplicantsPlaced { get; set; }

        [Required]
        [Display(Name = "Описание")]
        [StringLength(300, ErrorMessage = "Максимальное количество символов для описания вакансии — 300.")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Требуемый опыт")]
        public RequiredExperience RequiredExperience { get; set; }

        [Required]
        [Display(Name = "Тип занятости")]
        public TypeOfEmployment TypeOfEmployment { get; set; }

        [Display(Name = "Номер компании")]
        public uint? CompanyId { get; set; }

        [Required]
        [Display(Name = "Статус")]
        public Status Status { get; set; }
        public string GetDisplayName(Enum val)
        {
            return val.GetType()
                      .GetMember(val.ToString())
                      .FirstOrDefault()
                      ?.GetCustomAttribute<DisplayAttribute>(false)
                      ?.Name
                      ?? val.ToString();
        }
        //public static string GetDescription(this Enum value)
        //{
        //    FieldInfo fi = value.GetType().GetField(value.ToString());

        //    DescriptionAttribute[] attributes =
        //        (DescriptionAttribute[])fi.GetCustomAttributes(
        //        typeof(DescriptionAttribute),
        //        false);

        //    if (attributes != null &&
        //        attributes.Length > 0)
        //        return attributes[0].Description;
        //    else
        //        return value.ToString();
        //}
    }
}
