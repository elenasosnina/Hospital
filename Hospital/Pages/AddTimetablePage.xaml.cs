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
        private dynamic _selectedItem = null;
        public AddTimetablePage()
        {
            InitializeComponent();
        }
        public AddTimetablePage(dynamic selectedBookItem)
        {
            InitializeComponent();
            _selectedItem = selectedBookItem;
            FillFormFields();

        }
        private void FillFormFields()
        {
            if (_selectedItem != null)
            {
                cabinet.Text = _selectedItem.Кабинет;
                speciality.Text = _selectedItem.Специальность;
                days.Text = _selectedItem.День_недели;
                int item = _selectedItem.ID_расписание;
                var docID = Context.DB.Расписание_врачей.Where(ba => ba.ID_расписание == item).Select(ba => ba.ID_врача).ToList();
                var doc = Context.DB.Врачи
                    .Where(a => docID.Contains(a.ID_врача))
                    .ToList();
                var docNames = doc.Select(a => $"{a.Имя} {a.Фамилия} {a.Отчество}");

                doctor.Text = string.Join(" ", docNames);

                var startTimes = Context.DB.Расписание_врачей
                    .Where(ba => ba.ID_расписание == item)
                    .Select(ba => ba.Время_начало)
                    .ToList();

                var endTimes = Context.DB.Расписание_врачей
                    .Where(ba => ba.ID_расписание == item)
                    .Select(ba => ba.Время_окончания)
                    .ToList();
                if (startTimes.Count > 0)
                {
                    string[] startTimeParts = startTimes.First().ToString().Split(':'); 
                    SH.Text = startTimeParts[0];
                    SM.Text = startTimeParts.Length > 1 ? startTimeParts[1] : "00"; 
                }

                if (endTimes.Count > 0)
                {
                    string[] endTimeParts = endTimes.First().ToString().Split(':'); 
                    EH.Text = endTimeParts[0];
                    EM.Text = endTimeParts.Length > 1 ? endTimeParts[1] : "00";
                }

            }

        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(doctor.Text) || string.IsNullOrWhiteSpace(speciality.Text) ||
                    string.IsNullOrWhiteSpace(cabinet.Text) || string.IsNullOrWhiteSpace(days.Text) ||
                    string.IsNullOrWhiteSpace(SH.Text) || string.IsNullOrWhiteSpace(SM.Text) ||
                    string.IsNullOrWhiteSpace(EH.Text) || string.IsNullOrWhiteSpace(EM.Text))
                {
                    throw new Exception("Все поля должны быть заполнены");
                }


                if (!int.TryParse(SH.Text.Trim(), out int starthours) ||
                    !int.TryParse(SM.Text.Trim(), out int startminutes) ||
                    !int.TryParse(EH.Text.Trim(), out int endhours) ||
                    !int.TryParse(EM.Text.Trim(), out int endminutes))
                {
                    throw new Exception("Часы и минуты должны быть числовыми значениями");
                }
                if (!int.TryParse(cabinet.Text.Trim(), out int cabinetNumber))
                {
                    throw new Exception("Поле 'Кабинет' должно содержать только числовые значения");
                }

                if (starthours < 0 || starthours > 23 || startminutes < 0 || startminutes > 59 ||
                    endhours < 0 || endhours > 23 || endminutes < 0 || endminutes > 59)
                {
                    throw new Exception("Часы должны быть в диапазоне от 0 до 23, а минуты от 0 до 59");
                }

                TimeSpan starttimeSpan = new TimeSpan(starthours, startminutes, 0);
                TimeSpan endtimeSpan = new TimeSpan(endhours, endminutes, 0);

                if (starttimeSpan >= endtimeSpan)
                {
                    throw new Exception("Время начала должно быть меньше времени окончания");
                }

                string[] parts = doctor.Text.Split(' ');
                if (parts.Length < 3)
                {
                    throw new Exception("Неверное представление данных. Убедитесь, что введено как минимум три слова");
                }

                string surname = parts[0].Trim();
                string name = parts[1].Trim();
                string patronymic = parts[2].Trim();

                var existingDoctor = Context.DB.Врачи.FirstOrDefault(a => a.Фамилия == surname && a.Имя == name && a.Отчество == patronymic);

                Врачи docEntity = new Врачи
                {
                    Фамилия = surname,
                    Имя = name,
                    Отчество = patronymic,
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

                if (existingDoctor != null)
                {
                    docEntity = existingDoctor;
                }

                if (_selectedItem != null)
                {
                    timetableEntity.ID_расписание = _selectedItem.ID_расписание;
                    docEntity.ID_врача = _selectedItem.ID_врача;
                    Update(timetableEntity, docEntity);
                }
                else
                {
                    Context.DB.Врачи.Add(docEntity);
                    Context.DB.Расписание_врачей.Add(timetableEntity);
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

        public void Update(Расписание_врачей timetable, Врачи doctor)
        {
            try
            {
                var existingdoc = Context.DB.Врачи.Find(doctor.ID_врача);
                var existingtimetable = Context.DB.Расписание_врачей.Find(timetable.ID_расписание);

                if (existingdoc != null && existingtimetable != null)
                {
                    existingtimetable.День_недели = timetable.День_недели.Trim();
                    existingtimetable.Время_начало = timetable.Время_начало;
                    existingtimetable.Время_окончания = timetable.Время_окончания;

                    existingdoc.Имя = doctor.Имя.Trim();
                    existingdoc.Фамилия = doctor.Фамилия.Trim();
                    existingdoc.Отчество = doctor.Отчество.Trim();
                    existingdoc.Кабинет = doctor.Кабинет.Trim();
                    existingdoc.Специальность = doctor.Специальность.Trim();

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
                    throw new Exception("Не удалось найти врача для обновления");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Doctor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (doctor.Text == "Введите Фамилию Имя Отчество")
                {
                doctor.Text = string.Empty;
                }
        }

        private void Doctor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(doctor.Text))
                {
                    doctor.Text = "Введите Фамилию Имя Отчество";
                }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
