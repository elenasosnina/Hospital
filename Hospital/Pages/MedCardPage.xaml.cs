using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Hospital.Windows;

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


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddCard form = new AddCard();
            form.Show();
            form.Closed += (s, args) =>
            {
                Page_Loaded(sender, e);
            };
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Проверка, выбран ли объект для удаления
            if (time_table.SelectedItem == null)
            {
                MessageBox.Show("Не выбран объект для удаления");
                return;
            }

            // Получаем выбранный объект
            dynamic selectedRecord = time_table.SelectedItem;

            // Предполагаем, что у объекта есть свойство ID_пациента
            int patientId = selectedRecord.ID_пациента;

            // Проверяем, что ID больше 0
            if (patientId > 0)
            {
                // 1. Получаем ID медицинских карт пациента
                var medicalCards = Context.DB.Медицинские_карты.Where(mc => mc.ID_пациента == patientId).ToList();

                // 2. Удаляем записи из таблицы "Болезни мед карты" для каждой медицинской карты
                foreach (var medicalCard in medicalCards)
                {
                    var diseaseCards = Context.DB.Болезни_Медицинская_карта.Where(bm => bm.ID_медицинской_карты == medicalCard.ID_медицинской_карты).ToList();
                    foreach (var diseaseCard in diseaseCards)
                    {
                        Context.DB.Болезни_Медицинская_карта.Remove(diseaseCard);
                    }
                    // Удаляем медицинскую карту
                    Context.DB.Медицинские_карты.Remove(medicalCard);
                }

                // 3. Удаляем пациента
                var patient = Context.DB.Пациенты.FirstOrDefault(p => p.ID_пациента == patientId);
                if (patient != null)
                {
                    Context.DB.Пациенты.Remove(patient);
                    Context.DB.SaveChanges();
                    MessageBox.Show("Пациент и связанные записи успешно удалены.");
                    Page_Loaded(sender, e); // Обновляем данные на странице
                }
                else
                {
                    MessageBox.Show("Пациент не найден.");
                }
            }
            else
            {
                MessageBox.Show("Некорректный ID пациента.");
            }
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedItem = time_table.SelectedItem;
            if (selectedItem != null)
            {
                // Создаем экземпляр CardPage и передаем выбранную книгу
                AddTimetablePage cardPage = new AddTimetablePage(selectedItem);

                // Создаем окно авторизации и передаем заголовок
                FormWindow authWindow = new FormWindow
                {
                    Title = "Изменить данные врача"
                };

                // Навигируем на CardPage
                authWindow.FormFrameWindow.Navigate(cardPage);
                authWindow.Show();
                authWindow.Closed += (s, args) =>
                {
                    Page_Loaded(sender, e);
                };
            }
            else
            {
                System.Windows.MessageBox.Show("Пожалуйста, выберите элемент для обновления.");
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
                             пациент.Фамилия,
                             пациент.Имя,
                             пациент.Отчество,
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
