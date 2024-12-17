using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
using System.Xml;
using Hospital;

namespace Hospital.Pages
{
    /// <summary>
    /// Логика взаимодействия для TimetablePage.xaml
    /// </summary>
    public partial class TimetablePage : Page
    {
        public TimetablePage()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (time_table.SelectedItem == null)
            {
                MessageBox.Show("Не выбран объект для удаления");
                return;
            }
            if (time_table.SelectedItem == null)
            {
                MessageBox.Show("Не удалось получить данные");
                return;
            }
            dynamic obj = time_table.SelectedItem;
            int id = obj.ID_расписание;

            if (id > 0)
            {
                var position = Context.DB.Расписание_врачей.FirstOrDefault(ba => ba.ID_расписание == id) ;
                Context.DB.Расписание_врачей.Remove(position);
                Context.DB.SaveChanges();
                Page_Loaded(sender, e);
            }
            else
            {
                MessageBox.Show("Книга не найдена");
            }
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Context.DB.Врачи.Load();
            Context.DB.Расписание_врачей.Load();
            var doctors = Context.DB.Врачи.Local.ToList();
            var timetable = Context.DB.Расписание_врачей.Local.ToList();
            var result = from doc in doctors
                         join timeTable in timetable on doc.ID_врача equals timeTable.ID_врача

                         select new
                         {
                             doc.Фамилия,
                             doc.Имя,
                             doc.Отчество,
                             doc.Специальность,
                             doc.Кабинет,
                             timeTable.День_недели,
                             timeTable.Время_начало,
                             timeTable.Время_окончания,
                             timeTable.ID_расписание
                         };
            time_table.ItemsSource = result.ToList();
        }

        private void Patients_Click(object sender, RoutedEventArgs e)
        {
            PatientsPage doc = new PatientsPage();
            MainWindow w = (MainWindow)Window.GetWindow(this);
            w.Title = doc.Title;
            w.MainFrame.Navigate(doc);
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
            string search = finder.Text; var searchResult = (from doc in Context.DB.Врачи.Local.ToList()
                                                             join timeTable in Context.DB.Расписание_врачей.Local.ToList() on doc.ID_врача equals timeTable.ID_врача
                                                             where doc.Фамилия.Contains(search) || doc.Имя.Contains(search)
                                                             select new
                                                             {
                                                                 doc.Фамилия,
                                                                 doc.Имя,
                                                                 doc.Отчество,
                                                                 doc.Специальность,
                                                                 doc.Кабинет,
                                                                 timeTable.День_недели,
                                                                 timeTable.Время_начало,
                                                                 timeTable.Время_окончания,
                                                             });
            time_table.ItemsSource = searchResult.ToList();
        }

    }
}
