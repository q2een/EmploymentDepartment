﻿using System.ComponentModel;

namespace EmploymentDepartment.BL
{
    public interface IFaculty : IIdentifiable
    {
        [DisplayName("ID")]
        int ID { get; set; }

        [DisplayName("Наименование")]
        string Name { get; set; }
    }
}
