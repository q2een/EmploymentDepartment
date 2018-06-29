﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EmploymentDepartment.BL
{
    public class Student : IIdentifiable
    {
        [Browsable(false)]
        public int ID { get; set; }

        [DisplayName("Шифр анкеты")]
        public string ApplicationFormNumber { get; set; }

        [DisplayName("Фамилия")]
        public string Surname { get; set; }

        [DisplayName("Имя")]
        public string Name { get; set; }

        [DisplayName("Отчество")]
        public string Patronymic { get; set; }

        [DisplayName("Дата рождения")]
        public DateTime DOB { get; set; }

        [DisplayName("Пол")]
        public GenderType Gender { get; set; }

        [DisplayName("Семейное положение ")]
        public bool MaritalStatus { get; set; }

        [DisplayName("Год окончания университета")]
        public int YearOfGraduation { get; set; }

        [DisplayName("Факультет")]
        public int Faculty { get; set; }

        [Browsable(false)]
        public EducationLevelType LevelOfEducation { get; set; }

        [DisplayName("Уровень образования")]
        public string EducationLevel
        {
            get
            {
                switch (LevelOfEducation)
                {
                    case EducationLevelType.Bachelor:
                        return "Бакалавриат";
                    case EducationLevelType.Specialist:
                        return "Специалитет";
                    default:
                        return "Магистратура";
                }
            }
        }

        [DisplayName("Профиль(Специализация)")]
        public int FieldOfStudy { get; set; }

        [DisplayName("Группа")]
        public string StudyGroup { get; set; }

        [DisplayName("Рейтинг")]
        public decimal Rating { get; set; }

        [DisplayName("Льготная категория")]
        public string PreferentialCategory { get; set; }

        [DisplayName("Самостоятельное трудоустройство")]
        public bool SelfEmployment { get; set; }

        [DisplayName("Город (проживание)")]
        public string City { get; set; }

        [DisplayName("Область (проживание)")]
        public string Region { get; set; }

        [DisplayName("Район (проживание)")]
        public string District { get; set; }

        [DisplayName("Адрес (проживание)")]
        public string Address { get; set; }

        [DisplayName("Город (регистрация)")]
        public string RegCity { get; set; }

        [DisplayName("Область (регистрация)")]
        public string RegRegion { get; set; }

        [DisplayName("Район (регистрация)")]
        public string RegDistrict { get; set; }

        [DisplayName("Адрес (регистрация)")]
        public string RegAddress { get; set; }

        [DisplayName("Телефон")]
        public string Phone { get; set; }

        [DisplayName("Электронный адрес")]
        public string Email { get; set; }
    }
}