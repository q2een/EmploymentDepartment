﻿using EmploymentDepartment.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EmploymentDepartment
{
    public partial class VacancyForm : BaseVacancyForm, IVacancy, ILinkPickable
    {
        public ICompany LinkCompany
        {
            get
            {
                return linkCompany.Tag as ICompany;
            }
            private set
            {
                linkCompany.Tag = value;
                string text = value == null ? "Выбрать  предприятие..." : $"{value.Name}";
                linkCompany.Text = Extentions.ShortenString(text, companyPanel.Width - linkClear.Width - 90, linkCompany.Font);

                linkClear.Visible = Type != ActionType.View;   
            }
        }

        #region IVacancy implementation

        public new string VacancyNumber
        {
            get
            {
                return tbVacancyNumber.Text;
            }

            set
            {
                tbVacancyNumber.Text = value;
            }
        }

        public new string Post
        {
            get
            {
                return tbPost.Text;
            }

            set
            {
                tbPost.Text = value;
            }
        }

        public new int Employer
        {
            get
            {
                return LinkCompany.ID;
            }

            set
            {
                LinkCompany = main?.EntGetter?.GetCompanyById(value);
            }
        }

        public new string WorkArea
        {
            get
            {
                return tbWorkArea.Text;
            }

            set
            {
                tbWorkArea.Text = value;
            }
        }

        public new decimal Salary
        {
            get
            {
                decimal value;
                if (Decimal.TryParse(tbSalary.Text, out value))
                    return value;

                return 0;
            }

            set
            {
                tbSalary.Text = value.ToString();
            }
        }

        public new string SalaryNote
        {
            get
            {
                return tbSalaryNote.Text;
            }

            set
            {
                tbSalaryNote.Text = value;
            }
        }

        public new int Gender
        {
            get
            {
                return cmbGender.SelectedIndex + 1;
            }

            set
            {
                cmbGender.SelectedIndex = value - 1;
            }
        }

        public new string Features
        {
            get
            {
                return tbFeatures.Text;
            }

            set
            {
                tbFeatures.Text = value;
            }
        }

        #endregion

        public VacancyForm(ActionType type, IVacancy entity = null) : base (type, entity)
        {
            InitializeComponent();
        }

        public VacancyForm(MainMDIForm mainForm, IVacancy entity) : base(mainForm, entity)
        {
            InitializeComponent();
        }

        private void linkCompany_Validating(object sender, CancelEventArgs e)
        {
            if (LinkCompany == null)
                errorProvider.SetError(linkCompany, "Необходимо выбрать предприятие");
            else
                errorProvider.SetError(linkCompany, "");
        }

        #region IEditable interfaces implemantation.
        public void Remove()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Обработка событий для выбора предприятия.
        // Нажатие на элемент управление для выбора льготной категории. Обработка события.
        private void linkCompany_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (LinkCompany == null)
            {
                var form = new DataViewForm<Company>(main.EntGetter.GetCompanies(), main, this);
                form.ShowDialog(this);
                return;
            }

            main.ShowFormByType(ActionType.View, LinkCompany);
        }

        // Нажатие на элемент управления "Очистить". Обработка события.
        private void linkClear_Click(object sender, EventArgs e)
        {
            LinkCompany = null;
        }

        // Обработка события нажития клавиши при активном элементе для выбора льготной категории. Обработка события.
        private void linkCompany_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Открыть окно на space.
            if (e.KeyCode == Keys.Space)
            {
                linkCompany_LinkClicked(sender, null);
                return;
            }

            // Очистить на backspace and delete.
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                linkClear_Click(sender, null);
                return;
            }
        }
        #endregion

        public void SetLinkValue<T>(T obj) 
        {
            if (!(obj is ICompany))
                return;

            LinkCompany = obj as ICompany;
        }

        protected override ErrorProvider GetErrorProvider()
        {
            return errorProvider;
        }

        private void tbSalary_Validating(object sender, CancelEventArgs e)
        {
            RequiredTextBox_Validating(sender, e);
        }

        private void tbSalaryDecimal_Validating(object sender, CancelEventArgs e)
        {
            decimal value;
            if (!Decimal.TryParse(tbSalary.Text, out value))
                GetErrorProvider().SetError(tbSalary, "Введите корректное значение");
            else
                GetErrorProvider().SetError(tbSalary, "");
        }

        private void cmbGender_Validating(object sender, CancelEventArgs e)
        {
            RequiredComboBox_Validating(sender, e);
        }

        private void VacancyForm_Load(object sender, EventArgs e)
        {
            mainPanel.Enabled = Type != ActionType.View;
        }

    }
}
