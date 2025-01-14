﻿using Humanizer;
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
        PartTime = 2,
        [Display(Name = "СТажировка")]
        Internship = 3
    }
    public enum StatusforResume
    {
        [Display(Name = "Рассматриваемое менеджером")]
        ConsideredByTheManager = 1,
        [Display(Name = "Активное")] Active = 2,
        [Display(Name = "Неактивное")] Inactive = 3,
        [Display(Name = "Принятое")] Accepted = 4,
        [Display(Name = "Закрытое")] Closed = 5,
    }
    public class Resume
    {
        [Display(Name = "Номер")]
        public uint Id { get; set; }

        [Required]
        [Display(Name = "Номер должности")]
        public uint WorkPositionId { get; set; }

        [Required]
        [Display(Name = "Основное содержание резюме")]
        [StringLength(1000, ErrorMessage = "Максимальное количество символов для резюме — 1000.")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Опыт")]
        public Experience Experience { get; set; }

        [Required]
        [Display(Name = "Заявленный тип занятости")]
        public AdvertisedEmploymentType AdvertisedEmploymentType { get; set; }

        [Display(Name = "Номер вакансии")]
        public uint VacancyId { get; set; }

        [EmailAddress]
        [Display(Name = "Почта для связи")]
        public string? ApplicantMail { get; set; }

        [Display(Name = "Номер работодателя, которому рекомендовано резюме")]
        public string? RecommendedToEmployerId { get; set; }

        [Required]
        [Display(Name = "Статус")]
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
