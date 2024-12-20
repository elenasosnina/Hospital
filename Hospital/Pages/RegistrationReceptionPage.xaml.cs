using Hospital.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hospital.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationReceptionPage.xaml
    /// </summary>
    public partial class RegistrationReceptionPage : Page
    {
        public RegistrationReceptionPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Context.DB.Запись_на_приемы.Load();
            Context.DB.Пациенты.Load();
            Context.DB.Врачи.Load();
            Context.DB.Услуги.Load();
            var notes = Context.DB.Запись_на_приемы.Local.ToList();
            var patients = Context.DB.Пациенты.Local.ToList();
            var docs = Context.DB.Врачи.Local.ToList();
            var services = Context.DB.Услуги.Local.ToList();

            var result = from note in notes
                         join patient in patients on note.ID_пациента equals patient.ID_пациента
                         join doc in docs on note.ID_врача equals doc.ID_врача
                         join service in services on note.ID_услуги equals service.ID_услуги
                         select new
                         {
                             note.ID_записи_на_прием,
                             note.Дата_и_время,
                             note.Описание,
                             patient.ID_пациента,
                             patient.Фамилия,
                             patient.Имя,
                             patient.Отчество,
                             doc.ID_врача,
                             Фамилия_доктора = doc.Фамилия,
                             Имя_доктора = doc.Имя,
                             Отчество_доктора = doc.Отчество,
                             doc.Специальность,
                             service.ID_услуги,
                             service.Название
                         };
            table.ItemsSource = result.ToList();
        }
        private void MedCard_Click(object sender, RoutedEventArgs e)
        {
            MedCardPage card = new MedCardPage();
            MainWindow w = (MainWindow)Window.GetWindow(this);
            w.Title = card.Title;
            w.MainFrame.Navigate(card);
        }
        private void Timetable_Click(object sender, RoutedEventArgs e)
        {
            TimetablePage servicce = new TimetablePage();
            MainWindow w = (MainWindow)Window.GetWindow(this);
            w.Title = servicce.Title;
            w.MainFrame.Navigate(servicce);
        }

        private void Services_Click(object sender, RoutedEventArgs e)
        {
            ServicesPage servicce = new ServicesPage();
            MainWindow w = (MainWindow)Window.GetWindow(this);
            w.Title = servicce.Title;
            w.MainFrame.Navigate(servicce);
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            FormWindow form = new FormWindow();
            RegReceptionFormPage page = new RegReceptionFormPage();
            form.FormFrameWindow.Navigate(page);
            form.Show();
            form.Closed += (s, args) =>
            {
                Page_Loaded(sender, e);
            };
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (table.SelectedItem == null)
            {
                MessageBox.Show("Не выбран объект для удаления");
                return;
            }
            if (table.SelectedItem == null)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            dynamic obj = table.SelectedItem;
            int id = obj.ID_записи_на_прием;
            if (id > 0)
            {
                var position = Context.DB.Запись_на_приемы.FirstOrDefault(ba => ba.ID_записи_на_прием == id);
                Context.DB.Запись_на_приемы.Remove(position);
                Context.DB.SaveChanges();
                Page_Loaded(sender, e);
            }
            else
            {
                MessageBox.Show("Услуга указана в записи на прием");
            }
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedItem = table.SelectedItem;
            if (selectedItem != null)
            {
                RegReceptionFormPage page = new RegReceptionFormPage(selectedItem);
                FormWindow form = new FormWindow
                {
                    Title = "Изменить данные об услуге"
                };

                form.FormFrameWindow.Navigate(page);
                form.Show();
                form.Closed += (s, args) =>
                {
                    Page_Loaded(sender, e);
                };
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент для обновления");
            }
        }

        private void Find_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = finder.Text;
            if (string.IsNullOrEmpty(search))
            {
                Page_Loaded(sender, e);
            }
        }
        private void Search_Button(object sender, RoutedEventArgs e)
        {
            string search = finder.Text;
            var searchResult = (from note in Context.DB.Запись_на_приемы.Local.ToList()
                                join patient in Context.DB.Пациенты.Local.ToList() on note.ID_пациента equals patient.ID_пациента
                                join doc in Context.DB.Врачи.Local.ToList() on note.ID_врача equals doc.ID_врача
                                join service in Context.DB.Услуги.Local.ToList() on note.ID_услуги equals service.ID_услуги
                                where patient.Фамилия.Contains(search) || patient.Имя.Contains(search) ||
                                patient.Отчество.Contains(search) ||
                                doc.Фамилия.Contains(search) ||
                                doc.Имя.Contains(search) ||
                                doc.Отчество.Contains(search) ||
                                service.Название.Contains(search)
                                select new
                                {
                                    note.ID_записи_на_прием,
                                    note.Дата_и_время,
                                    note.Описание,
                                    patient.Фамилия,
                                    patient.Имя,
                                    patient.Отчество,
                                    Фамилия_доктора = doc.Фамилия,
                                    Имя_доктора = doc.Имя,
                                    Отчество_доктора = doc.Отчество,
                                    doc.Специальность,
                                    service.Название
                                });
            table.ItemsSource = searchResult.ToList();
        }
    }
}
