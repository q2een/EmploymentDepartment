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
    public partial class StudentCompanyForm : BaseStudentCompanyForm, IStudentCompany, ILinkPickable
    {
        public StudentCompanyForm(ActionType type, IStudentCompany entity = null):base(type, entity)
        {
            InitializeComponent();
        }

        public StudentCompanyForm(IStudentCompany entity, IStudent student) : base(ActionType.Add, entity)
        {
            InitializeComponent();
            LinkStudent = student;
            linkStudent.Enabled =linkStudentClear.Visible = false;
        }

        public StudentCompanyForm(IStudentCompany entity, IVacancy vacancy) : base(ActionType.Add, entity)
        {
            InitializeComponent();
            LinkVacancy = vacancy;
            linkVacancy.Enabled = linkVacancyClear.Visible = false;
        }

        public StudentCompanyForm(ActionType type, IStudentCompany entity, IDataListView<IStudentCompany> viewContext) : base(type, entity, viewContext)
        {
            InitializeComponent();
        }
        
        // Модальное окно для просмотра информации.
        public StudentCompanyForm(MainMDIForm mainForm, IStudentCompany entity) : base(mainForm, entity)
        {
            InitializeComponent();
        }

        public IStudent LinkStudent
        {
            get
            {
                return linkStudent.Tag as IStudent;
            }
            set
            {                   
                linkStudent.Tag = value;
                string text = value == null ? "Выбрать студента ..." : $"{value.Surname} {value.Name} {value.Patronymic}";
                linkStudent.Text = Extentions.ShortenString(text, studentPanel.Width - linkStudentClear.Width - 90, linkStudent.Font);

                linkStudentClear.Visible = Type != ActionType.View;
            }
        }

        public IVacancy LinkVacancy
        {
            get
            {
                return linkVacancy.Tag as IVacancy;
            }
            set
            {
                if(value != null)
                    cbUnivercityEmployment.Checked = true;

                linkVacancy.Tag = value;
                string text = value == null ? "Выбрать вакансию ..." : $"Вакансия №{value.VacancyNumber}";
                linkVacancy.Text = Extentions.ShortenString(text, vacancyPanel.Width - linkVacancyClear.Width - 90, linkVacancy.Font);
                var compnany = value == null ? null : Main.Entities.GetSingle<Company>(value.Employer);

                tbCompany.Text = compnany == null ? "" : compnany?.Name;
                tbNameOfStateDepartment.Text = compnany == null ? "" : compnany?.NameOfStateDepartment;
                tbPost.Text = value?.Post;
                tbSalary.Text = value?.Salary.ToString();

                linkVacancyClear.Visible = Type != ActionType.View;
            }
        }

        private void cbUnivercityEmployment_CheckedChanged(object sender, EventArgs e)
        {
            vacancyPanel.Enabled = cbUnivercityEmployment.Checked;
            tbCompany.Enabled = !cbUnivercityEmployment.Checked;
            tbPost.Enabled = !cbUnivercityEmployment.Checked;
            tbSalary.Enabled = !cbUnivercityEmployment.Checked;
            tbNameOfStateDepartment.Enabled = !cbUnivercityEmployment.Checked; 

            if (!cbUnivercityEmployment.Checked)
            {
                LinkVacancy = null;
                tbCompany.Focus();
                errorProvider.SetError(linkVacancy, "");
            }
            else
            {
                linkVacancy.Focus();
                errorProvider.SetError(tbCompany, "");
                errorProvider.SetError(tbPost, "");
            }
        }

        private void tbSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',')
            {
                if (tbSalary.Text.Length == 0)
                {
                    e.Handled = true;
                    return;
                }

                e.Handled = tbSalary.Text.IndexOf(",") != -1;
                return;
            }

            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void StudentCompanyForm_Load(object sender, EventArgs e)
        {
            if (Type == ActionType.Add)
                cmbStatus.SelectedIndex = 0;

            if (Type == ActionType.View)
            {
                mainPanel.DisableControls(linkVacancy, linkStudent);
            }
        }

        // Устанавливает заголовок окна.
        protected override void SetFormText()
        {
            switch (Type)
            {
                case ActionType.Edit:
                    this.Text = $"Редактирование информации о месте работы студента";
                    break;
                case ActionType.Add:
                    this.Text = $"Добавление места работы студента";
                    break;
                case ActionType.View:
                    this.Text = $"Просмотр места работы. Студент: {LinkStudent.Surname} {LinkStudent.Name} {LinkStudent.Patronymic}";
                    break;
            }
        }

        protected override ErrorProvider GetErrorProvider()
        {
            return errorProvider;
        }

        public override void AddNewItem()
        {
            if (Vacancy != null)
            {
                var vacancies = Main.Entities.GetSudentsByVacancyID((int)Vacancy);

                if (vacancies != null && vacancies.Count() != 0)
                {
                    System.Windows.Forms.MessageBox.Show("Выбранную вакансию уже занимает другой студент. Укажите другую вакансию", "Операция добавления", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }
            }

            base.AddNewItem();
        }

        #region link elements events
        // Нажатие на элемент управления "Очистить". Обработка события.
        private void linkClear_Click(object sender, EventArgs e)
        {
            var link = sender as Label;

            if (link.Name == "linkStudent" || link.Name == "linkStudentClear") 
                LinkStudent = null;
            else
                LinkVacancy = null;
        }

        // Обработка события нажития клавиши при активном элементе для выбора льготной категории. Обработка события.
        private void link_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var link = sender as LinkLabel;

            // Открыть окно на space.
            if (e.KeyCode == Keys.Space)
            {
                if (link.Name == "linkStudent")
                    linkStudent_LinkClicked(sender, null);
                else
                    linkVacancy_LinkClicked(sender, null);              
                return;
            }

            // Очистить на backspace and delete.
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                linkClear_Click(sender, null);
                return;
            }
        }

        public void SetLinkValue<T>(T obj)
        {
            if (obj is IStudent)
                LinkStudent = obj as IStudent;
            if (obj is IVacancy)
                LinkVacancy = obj as IVacancy;
        }

        private void linkStudent_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (LinkStudent == null)
            {
                var form = new DataViewForm<Student>("Выбор студента", Main.Entities.GetEntities<Student>(), Main, this);
                form.ShowDialog(this);
                return;
            }

            Main.ShowFormByType(ActionType.View, LinkStudent);
        }

        private void linkVacancy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (LinkVacancy == null)
            {
                var form = new DataViewForm<Vacancy>("Выбор вакансии", Main.Entities.GetEntities<Vacancy>(), Main, this);
                form.ShowDialog(this);
                return;
            }

            Main.ShowFormByType(ActionType.View, LinkVacancy);
        }
        #endregion

        #region Validating
        // Валадация текстового поля "Год трудоустройства".
        private void tbYearOfEmployment_Validating(object sender, CancelEventArgs e)
        {
            int year;

            if (!Int32.TryParse(tbYearOfEmployment.Text, out year))
                errorProvider.SetError(tbYearOfEmployment, "Укажите корректный год трудоустройства");
            else
                if (year < 2018)
                errorProvider.SetError(tbYearOfEmployment, "Год трудоустройства должен быть больше 2018");
            else
                errorProvider.SetError(tbYearOfEmployment, "");
        }

        private void linkStudent_Validating(object sender, CancelEventArgs e)
        {
            if (LinkStudent == null)
                errorProvider.SetError(linkStudent, "Необходимо выбрать студента");
            else
                errorProvider.SetError(linkStudent, "");
        }

        private void linkVacancy_Validating(object sender, CancelEventArgs e)
        {
            if (LinkVacancy == null)
                errorProvider.SetError(linkVacancy, "Необходимо выбрать вакансию");
            else
                errorProvider.SetError(linkVacancy, "");
        }

        private void TBRequire_Validating(object sender, CancelEventArgs e) => this.RequiredTextBox_Validating(sender, e);

        private void cmbStatus_Validating(object sender, CancelEventArgs e) => this.RequiredComboBox_Validating(sender, e);

        #endregion

        #region IStudentCompany implementation
        
        public new string StudentFullName
        {
            get
            {
                return LinkStudent == null ? null : $"{LinkStudent.Surname} {LinkStudent.Name} {LinkStudent.Patronymic}".Trim();
            }
        }

        public new int Student
        {
            get
            {
                return LinkStudent.ID;
            }
            set
            {
                LinkStudent = Main?.Entities?.GetSingle<Student>(value);
            }
        }

        public new int? Vacancy
        {
            get
            {
                return LinkVacancy?.ID;
            }

            set
            {
                LinkVacancy = value == null ? null : Main?.Entities?.GetSingle<Vacancy>((int)value);
            }
        }
        
        public new string NameOfStateDepartment
        {
            get
            {
                return string.IsNullOrEmpty(tbNameOfStateDepartment.Text.Trim()) ? null : tbNameOfStateDepartment.Text;
            }

            set
            {
                tbNameOfStateDepartment.Text = value;
            }
        }
      
        public new string NameOfCompany
        {
            get
            {
                return tbCompany.Text;
            }
            set
            {
                tbCompany.Text = value;
            }
        }

        public new bool Status
        {
            get
            {
                return cmbStatus.SelectedIndex == 0 ? true : false;
            }

            set
            {
                cmbStatus.SelectedIndex = value ? 0 : 1;
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

        public new string StatusText
        {
            get
            {
                return cmbStatus.Text;
            }
        }

        public new decimal? Salary
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
         
        public new string VacancyNumber
        {
            get
            {
                return LinkVacancy?.VacancyNumber;
            }
        }

        public new int YearOfEmployment
        {
            get
            {
                int year;
                if (!Int32.TryParse(tbYearOfEmployment.Text, out year))
                    return 0;

                return year;
            }

            set
            {
                tbYearOfEmployment.Text = value.ToString();
            }
        }
         
        public new string Note
        {
            get
            {
                return string.IsNullOrEmpty(tbNote.Text.Trim()) ? null : tbNote.Text;
            }

            set
            {
                tbNote.Text = value;
            }
        }
         
        #endregion
    }
}
