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

namespace Hospital.Pages
{
    /// <summary>
    /// Логика взаимодействия для MedCardPage.xaml
    /// </summary>
    public partial class MedCardPage : Page
    {
        public MedCardPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Context.DB.Пациенты.Load();
            Context.DB.Болезни.Load();
            Context.DB.Болезни_Медицинская_карта.Load();
            Context.DB.Медицинские_карты.Load();

            var пациенты = Context.DB.Пациенты.Local.ToList();
            var болезни = Context.DB.Болезни.Local.ToList();
            var болезниМедКарты = Context.DB.Болезни_Медицинская_карта.Local.ToList();
            var медицинскиеКарты = Context.DB.Медицинские_карты.Local.ToList();

            var result = from пациент in пациенты
                         join медКарта in медицинскиеКарты on пациент.ID_пациента equals медКарта.ID_пациента
                         join болезньМедКарта in болезниМедКарты on медКарта.ID_медицинской_карты equals болезньМедКарта.ID_медицинской_карты
                         join болезнь in болезни on болезньМедКарта.ID_болезни equals болезнь.ID_болезни
                         select new
                         {
                             Пациент=$"{пациент.Фамилия} {пациент.Имя} {пациент.Отчество}",
                             пациент.Дата_рождения,
                             пациент.Пол,
                             медКарта.Номер_карты,
                             медКарта.Группа_крови,
                             болезнь.Название,
                             болезньМедКарта.Результат_болезни
                         };

            time_table.ItemsSource = result.ToList();
        }



        private void Timetable_Click(object sender, RoutedEventArgs e)
        {
            TimetablePage table = new TimetablePage();
            MainWindow w = (MainWindow)Window.GetWindow(this);
            w.Title = table.Title;
            w.MainFrame.Navigate(table);
        }

        private void Services_Click(object sender, RoutedEventArgs e)
        {
            ServicesPage servicce = new ServicesPage();
            MainWindow w = (MainWindow)Window.GetWindow(this);
            w.Title = servicce.Title;
            w.MainFrame.Navigate(servicce);
        }

        private void RegistrationReception_Click(object sender, RoutedEventArgs e)
        {
            RegistrationReceptionPage reg = new RegistrationReceptionPage();
            MainWindow w = (MainWindow)Window.GetWindow(this);
            w.Title = reg.Title;
            w.MainFrame.Navigate(reg);
        }
    }

}
