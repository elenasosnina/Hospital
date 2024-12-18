using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
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
    /// Логика взаимодействия для AddTimetablePage.xaml
    /// </summary>
    public partial class AddTimetablePage : Page
    {
        public AddTimetablePage()
        {
            InitializeComponent();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int starthours = int.Parse(SH.Text.Trim());
                int startminutes = int.Parse(SM.Text.Trim());
                int startseconds = 0;
                TimeSpan starttimeSpan = new TimeSpan(starthours, startminutes, startseconds);
                int endhours = int.Parse(EH.Text.Trim());
                int endminutes = int.Parse(EM.Text.Trim());
                int endseconds = 0;
                TimeSpan endtimeSpan = new TimeSpan(endhours, endminutes, endseconds);
                Врачи docEntity;

                string[] parts = doctor.Text.Split(' ');
                if (parts.Length < 3)
                {
                    throw new Exception("Неверное представление данных. Убедитесь, что введено как минимум три слова.");
                }
                
                string surname = parts[0].Trim();
                string name = parts[1].Trim();
                string patronymic = parts[2].Trim();
                var existingDoctor = Context.DB.Врачи.FirstOrDefault(a => a.Фамилия == surname && a.Имя == name && a.Отчество == patronymic);
                if (existingDoctor != null)
                {
                    docEntity = existingDoctor;

                }
                else
                {
                    docEntity = new Врачи
                    {
                        Фамилия = parts[0].Trim(),
                        Имя = parts[1].Trim(),
                        Отчество = parts[2].Trim(),
                        Специальность = speciality.Text.Trim(),
                        Кабинет = cabinet.Text.Trim()

                    };
                    
                    Расписание_врачей timetableEntity = new Расписание_врачей
                    {
                        ID_врача = docEntity.ID_врача,
                        День_недели = days.Text.Trim(),
                        Время_начало = starttimeSpan,
                        Время_окончания = endtimeSpan

                    };
                    Context.DB.Врачи.Add(docEntity);
                    Context.DB.Расписание_врачей.Add(timetableEntity);
                    Context.DB.SaveChanges();
                    MessageBox.Show("Запись успешно сохранена.");
                    Window parentWindow = Window.GetWindow(this);
                    parentWindow?.Close();
                }

                //if (_selectedItem != null)
                //{
                //    bookEntity.BookID = _selectedItem.BookID;

                //    Update(bookEntity, _selectedItem.AuthorID, authorEntity, selectedGenre);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void doctor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (doctor.Text == "Введите Фамилию Имя Отчество")
                {
                doctor.Text = string.Empty;
                }
        }

        private void doctor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(doctor.Text))
                {
                    doctor.Text = "Введите Фамилию Имя Отчество";
                }
        }
    }
}
