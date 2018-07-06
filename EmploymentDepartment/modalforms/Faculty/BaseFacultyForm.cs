﻿using EmploymentDepartment.BL;
using System;

namespace EmploymentDepartment
{
    public class BaseFacultyForm : MDIChild<IFaculty>, IFaculty
    {
        public BaseFacultyForm() : base()
        {
        }

        public BaseFacultyForm(ActionType type, IFaculty entity = null) : base(type, entity)
        {
        }

        public BaseFacultyForm(MainMDIForm mainForm, IFaculty entity) : base(mainForm, entity)
        {
        }

        protected override void SetFormText()
        {
            switch (Type)
            {
                case ActionType.Edit:
                    this.Text = $"Редактирование информации о факультете [{Entity.Name}]";
                    break;
                case ActionType.Add:
                    this.Text = $"Укажите наименование факультета";
                    break;
                case ActionType.View:
                    this.Text = $"[{Entity.Name}] - Просмотр информации о факультете";
                    break;
            }
        }

        #region IFaculty
        string IFaculty.Name { get; set; }
        #endregion

        #region IEditable implementation.

        public override bool ValidateFields() => Extentions.ValidateFields(this, GetErrorProvider());

        public override void SetDefaultValues() => this.SetPropertiesValue<IFaculty>(Entity, "");

        public override void Save()
        {
            var msg = $"Информация о факультете\nНаименование факультета: {((IFaculty)this).Name}";

            if (this.UpdateFormEntityInDataBase<BaseFacultyForm, IFaculty>(main.DBGetter, msg, "ID"))
            {
                SetFormText();
                main.UpdateFaculties();
                main.UpdateSpecializations();
                this.Close();
            }
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }

        public override void Insert()
        {
            var msg = $"Факультет добавлен в базу.\nНаименование факультета: {((IFaculty)this).Name}";

            if (this.UpdateFormEntityInDataBase<BaseFacultyForm, IFaculty>(main.DBGetter, msg, "ID"))
            {
                SetFormText();
                main.UpdateFaculties();
                main.UpdateSpecializations();
                this.Close();
            }
        }

        #endregion
    }
}