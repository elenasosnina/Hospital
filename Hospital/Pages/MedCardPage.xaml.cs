using Hospital.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
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
    public partial class MedCardPage : Page
    {
        public MedCardPage()
        {
            InitializeComponent();
            if (CurrentUser.RoleID == 3)
            {
                add.Visibility = Visibility.Hidden;
                add.IsEnabled = false;
                del.Visibility = Visibility.Hidden;
                del.IsEnabled = false;
                update.Visibility = Visibility.Hidden;
                update.IsEnabled = false;
            }
            if (CurrentUser.RoleID == 2)
            {
                add.Visibility = Visibility.Hidden;
                add.IsEnabled = false;
                del.Visibility = Visibility.Hidden;
                del.IsEnabled = false;


            }
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
                             пациент.ID_пациента,
                             медКарта.Номер_карты,
                             медКарта.Группа_крови,
                             медКарта.ID_медицинской_карты,
                             болезнь.Название,
                             болезньМедКарта.Результат_болезни
                         };

            time_table.ItemsSource = result.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            FormWindow form = new FormWindow();
            MedCardFormPage page = new MedCardFormPage();
            form.FormFrameWindow.Navigate(page);
            form.Show();
            form.Closed += (s, args) =>
            {
                Page_Loaded(sender, e);
            };
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            dynamic selectedRecord = time_table.SelectedItem;
            int patientId = selectedRecord.ID_пациента;

            if (patientId > 0)
            {
                var medicalCards = Context.DB.Медицинские_карты.Where(mc => mc.ID_пациента == patientId).ToList();
                foreach (var medicalCard in medicalCards)
                {
                    var diseaseCards = Context.DB.Болезни_Медицинская_карта.Where(bm => bm.ID_медицинской_карты == medicalCard.ID_медицинской_карты).ToList();
                    foreach (var diseaseCard in diseaseCards)
                    {
                        Context.DB.Болезни_Медицинская_карта.Remove(diseaseCard);
                    }
                    Context.DB.Медицинские_карты.Remove(medicalCard);
                }

                var patient = Context.DB.Пациенты.FirstOrDefault(p => p.ID_пациента == patientId);
                if (patient != null)
                {
                    Context.DB.Пациенты.Remove(patient);
                    Context.DB.SaveChanges();
                    MessageBox.Show("Пациент и связанные записи успешно удалены");
                    Page_Loaded(sender, e); 
                }
                else
                {
                    MessageBox.Show("Пациент не найден");
                }
            }
            else
            {
                MessageBox.Show("Некорректный ID пациента");
            }
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedItem = time_table.SelectedItem;
            if (selectedItem != null)
            {
                MedCardFormPage cardPage = new MedCardFormPage(selectedItem);

                FormWindow authWindow = new FormWindow
                {
                    Title = "Изменить данные медицинской карты"
                };

                authWindow.FormFrameWindow.Navigate(cardPage);
                authWindow.Show();
                authWindow.Closed += (s, args) =>
                {
                    Page_Loaded(sender, e);
                };
            }
            else
            {
                System.Windows.MessageBox.Show("Пожалуйста, выберите элемент для обновления");
            }

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
