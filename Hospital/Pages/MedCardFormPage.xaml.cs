using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MedCardFormPage.xaml
    /// </summary>
    public partial class MedCardFormPage : Page
    {
        private dynamic _selectedItem = null;
        public MedCardFormPage()
        {
            InitializeComponent();
        }
        public MedCardFormPage(dynamic selectedBookItem)
        {
            InitializeComponent();
            _selectedItem = selectedBookItem;
            //FillFormFields();

        }
        //private void FillFormFields()
        //{

        //    //patient.Text = selectedPatient.ФИО;
        //    //number_card.Text = selectedPatient.НомерКарты.ToString();
        //    //day.Text = selectedPatient.ДатаРождения.ToShortDateString();
        //    //phone.Text = selectedPatient.Телефон;
        //    //gender.Text = selectedPatient.Пол;
        //    //group_blood.Text = selectedPatient.ГруппаКрови;
        //    //dieasis.Text = selectedPatient.Болезнь;
        //    //result.Text = selectedPatient.Результат;

        //}

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string[] parts = patient.Text.Split(' ');
                if (parts.Length < 3)
                {
                    throw new Exception("Неверное представление данных. Убедитесь, что введено как минимум три слова");
                }

                string surname = parts[0].Trim();
                string name = parts[1].Trim();
                string patronymic = parts[2].Trim();


                var existingPatient = Context.DB.Пациенты.FirstOrDefault(a => a.Фамилия == surname && a.Имя == name && a.Отчество == patronymic);
                if (existingPatient != null)
                {
                    MessageBox.Show("Пациент уже существует");
                    return;
                }


                if (!DateTime.TryParse(day.Text.Trim(), out DateTime birthDate))
                {
                    throw new Exception("Неверный формат даты. Пожалуйста, введите дату в формате ДД.ММ.ГГГГ");
                }


                Пациенты пациент = new Пациенты
                {
                    Фамилия = surname,
                    Имя = name,
                    Отчество = patronymic,
                    Дата_рождения = birthDate,
                    Телефон = phone.Text.Trim(),
                    Пол = gender.Text.Trim(),
                    ID_пользователя = 1
                };

                Context.DB.Пациенты.Add(пациент);
                Context.DB.SaveChanges();

                MessageBox.Show("Пациент успешно добавлен!");
                Медицинские_карты медКарты = new Медицинские_карты
                {
                    ID_пациента = пациент.ID_пациента,
                    Номер_карты = number_card.Text.Trim(),
                    Группа_крови = group_blood.Text.Trim(),
                };

                Context.DB.Медицинские_карты.Add(медКарты);
                Context.DB.SaveChanges();

                Болезни болезни = new Болезни
                {
                    Название = dieasis.Text.Trim()
                };
                Context.DB.Болезни.Add(болезни);
                Context.DB.SaveChanges();

           
                Болезни_Медицинская_карта болезниМедКарты = new Болезни_Медицинская_карта
                {
                    ID_болезни = болезни.ID_болезни, 
                    ID_медицинской_карты = медКарты.ID_медицинской_карты, 
                    Результат_болезни = result.Text.Trim(),
                };

                Context.DB.Болезни_Медицинская_карта.Add(болезниМедКарты);
                Context.DB.SaveChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
        //public void Update(Услуги service, Стоимость_услуг costService)
        //{
        //    try
        //    {

        //        var existingservice = Context.DB.Услуги.Find(service.ID_услуги);
        //        var existingcost = Context.DB.Стоимость_услуг.Find(costService.ID_стоимости_услуги);

        //        if (existingservice != null && existingcost != null)
        //        {

        //            existingservice.Название = service.Название.Trim();
        //            existingcost.Стоимость = costService.Стоимость;
        //            existingcost.Скидка = costService.Скидка;

        //            Context.DB.SaveChanges();
        //            MessageBox.Show("Запись успешно обновлена");
        //            var window = Window.GetWindow(this);
        //            if (window != null)
        //            {
        //                window.Close();
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Не удалось найти медицинскую карту для обновления");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

    }
}
