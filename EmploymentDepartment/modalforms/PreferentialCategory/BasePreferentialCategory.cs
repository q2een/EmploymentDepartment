﻿using EmploymentDepartment.BL;
using System;

namespace EmploymentDepartment
{
    public class BasePreferentialCategory : MDIChild<IPreferentialCategory>, IPreferentialCategory
    {
        private BasePreferentialCategory() 
        {
        }

        public BasePreferentialCategory(ActionType type, IPreferentialCategory entity = null) : base(type, entity)
        {
        }

        public BasePreferentialCategory(ActionType type, IPreferentialCategory entity, IDataListView<IPreferentialCategory> viewContext):base(type,entity,viewContext)
        { 
        }

        public BasePreferentialCategory(MainMDIForm mainForm, IPreferentialCategory entity) : base(mainForm, entity)
        {
        }

        protected override void SetFormText()
        {
            switch (Type)
            {
                case ActionType.Edit:
                    this.Text = $"Редактирование информации о льготной категории";
                    break;
                case ActionType.Add:
                    this.Text = $"Укажите текст льготной категории";
                    break;
                case ActionType.View:
                    this.Text = $"Просмотр информации о льготной категории";
                    break;
            }
        }

        protected override string[] IngnoreProperties
        {
            get
            {
                return new string[] { "ID" };
            }
        }

        #region IPreferentialCategory
        string IPreferentialCategory.Name { get; set; }
        #endregion

        #region IEditable implementation.

        public override bool ValidateFields() => Extentions.ValidateFields(this, GetErrorProvider());

        public override void SetDefaultValues() => this.SetPropertiesValue<IPreferentialCategory>(Entity, "");

        public override bool Save()
        {
            var msg = $"Информация о льготной категории обновлена";

            if (this.UpdateFormEntityInDataBase<BasePreferentialCategory, IPreferentialCategory>(Main.DataBase, msg, IngnoreProperties))
            {
                SetFormText();

                ViewContext?.SetDataTableRow(this as IPreferentialCategory);
                            
                this.Close();

                return true;
            }

            return false;
        }

        public override void AddNewItem()
        {
            var msg = $"Льготная категория добавлена в базу";
            if (this.InsertFormEntityToDataBase<BasePreferentialCategory, IPreferentialCategory>(Main.DataBase, msg, IngnoreProperties))
            {
                var viewForm = ViewContext ?? Main.GetDataViewForm<IPreferentialCategory>();
                viewForm?.SetDataTableRow(this as IPreferentialCategory);

                this.Close();
            }
        }

        #endregion
    }
}
