using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hospital.Pages
{
    public partial class RegReceptionFormPage : Page
    {
        private dynamic _selectedItem = null;
        public RegReceptionFormPage()
        {
            InitializeComponent();
            Context.DB.Услуги.Distinct().ToList();

            service.ItemsSource = Context.DB.Услуги.Local;
            service.DisplayMemberPath = "Название";
            service.SelectedValuePath = "ID_услуги";

            var doctors = Context.DB.Врачи.Distinct().ToList();
            doctor.ItemsSource = doctors;
            var patients = Context.DB.Пациенты.Distinct().ToList();
            patient.ItemsSource = patients;
        }
        public RegReceptionFormPage(dynamic selectedBookItem)
        {
            InitializeComponent();
            _selectedItem = selectedBookItem;
            FillFormFields();

        }
        private void FillFormFields()
        {
            if (_selectedItem != null)
            {
                description.Text = _selectedItem.Описание;
                int item = _selectedItem.ID_услуги;
                date.SelectedDate = _selectedItem.Дата_и_время;

                SH.Text = _selectedItem.Дата_и_время.Hour.ToString();
                SM.Text = _selectedItem.Дата_и_время.Minute.ToString();

                var services = Context.DB.Услуги.Where(g => g.ID_услуги == item).ToList();
                var titleservice = services.Select(a => a.Название);
                if (services.Any())
                {
                    service.ItemsSource = services;
                }
                else
                {
                    service.ItemsSource = null;
                }

                service.ItemsSource = Context.DB.Услуги.Local;
                service.DisplayMemberPath = "Название";

                if (service.SelectedItem == null && services.Count > 0)
                {
                    service.SelectedItem = services[0];
                }


                int itemdoc = _selectedItem.ID_врача;
                var docs = Context.DB.Врачи.Where(g => g.ID_врача == itemdoc).ToList();
              
                if (docs.Any())
                {
                    doctor.ItemsSource = docs;
                }
                else
                {
                    doctor.ItemsSource = null;
                }

                var doctors = Context.DB.Врачи.Distinct().ToList();
                doctor.ItemsSource = doctors;

                if (doctor.SelectedItem == null && docs.Count > 0)
                {
                    doctor.SelectedItem = docs[0];
                }



                int itemPatients = _selectedItem.ID_пациента;
                var patientsList = Context.DB.Пациенты.Where(g => g.ID_пациента == itemPatients).ToList();

                if (patientsList.Any())
                {
                    patient.ItemsSource = patientsList;
                }
                else
                {
                    patient.ItemsSource = null;
                }

                var patients = Context.DB.Пациенты.Distinct().ToList();
                patient.ItemsSource = patients;

                if (patient.SelectedItem == null && patientsList.Count > 0)
                {
                    patient.SelectedItem = patientsList[0];
                }

            }

        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Услуги selectedservice = service.SelectedItem as Услуги;
                Врачи selecteddoc = doctor.SelectedItem as Врачи;
                Пациенты selectedpatients = patient.SelectedItem as Пациенты;
                DateTime selectedDate = date.SelectedDate ?? DateTime.Now;

                if (!int.TryParse(SH.Text.Trim(), out int starthours) || starthours < 0 || starthours > 23)
                {
                    throw new Exception("Часы должны быть целым числом от 0 до 23");
                }

                if (!int.TryParse(SM.Text.Trim(), out int startminutes) || startminutes < 0 || startminutes > 59)
                {
                    throw new Exception("Минуты должны быть целым числом от 0 до 59");
                }

                DateTime dateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, starthours, startminutes, 0);

                if (selectedservice == null || selecteddoc == null || selectedpatients == null)
                {
                    throw new Exception("Не все поля заполнены. Пожалуйста, выберите услугу, врача и пациента");
                }

                Запись_на_приемы regreception = new Запись_на_приемы
                {
                    ID_врача = selecteddoc.ID_врача,
                    ID_пациента = selectedpatients.ID_пациента,
                    ID_услуги = selectedservice.ID_услуги,
                    Описание = description.Text.Trim(),
                    Дата_и_время = dateTime
                };

                if (_selectedItem != null)
                {
                    regreception.ID_записи_на_прием = _selectedItem.ID_записи_на_прием;
                    Update(regreception);
                }
                else
                {
                    Context.DB.Запись_на_приемы.Add(regreception);
                    Context.DB.SaveChanges();
                    MessageBox.Show("Запись успешно сохранена");
                    Window parentWindow = Window.GetWindow(this);
                    parentWindow?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Update(Запись_на_приемы regreception)
        {
            try
            {
                var existingnote = Context.DB.Запись_на_приемы.Find(regreception.ID_записи_на_прием);

                if (existingnote != null)
                {
                    existingnote.Дата_и_время = regreception.Дата_и_время;
                    existingnote.Описание = regreception.Описание;
                    existingnote.ID_врача = regreception.ID_врача;
                    existingnote.ID_услуги = regreception.ID_услуги;
                    existingnote.ID_пациента = regreception.ID_пациента;
                    Context.DB.SaveChanges();
                    MessageBox.Show("Запись успешно обновлена");
                    var window = Window.GetWindow(this);
                    if (window != null)
                    {
                        window.Close();
                    }
                }
                else
                {
                    throw new Exception("Не удалось запись для обновления");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
