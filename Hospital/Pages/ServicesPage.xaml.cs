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
using Hospital;
using Hospital.Windows;

namespace Hospital.Pages
{
    public partial class ServicesPage : Page
    {
        public ServicesPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Context.DB.Услуги.Load();
            Context.DB.Стоимость_услуг.Load();
            var service = Context.DB.Услуги.ToList();
            var price = Context.DB.Стоимость_услуг.Local.ToList();
            var result = from serviceitem in service
                         join priceitem in price on serviceitem.ID_услуги equals priceitem.ID_услуги

                         select new
                         {
                             serviceitem.ID_услуги,
                             serviceitem.Название,
                             priceitem.Стоимость,
                             priceitem.Скидка,
                             priceitem.ID_стоимости_услуги
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

        private void RegistrationReception_Click(object sender, RoutedEventArgs e)
        {
            RegistrationReceptionPage reg = new RegistrationReceptionPage();
            MainWindow w = (MainWindow)Window.GetWindow(this);
            w.Title = reg.Title;
            w.MainFrame.Navigate(reg);
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            FormWindow form = new FormWindow();
            ServiceFormPage page = new ServiceFormPage();
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
            int id = obj.ID_услуги;
            var proverka = Context.DB.Запись_на_приемы.FirstOrDefault(ba => ba.ID_услуги == id);

            if (proverka == null)
            {
                var position = Context.DB.Услуги.FirstOrDefault(ba => ba.ID_услуги == id);
                Context.DB.Услуги.Remove(position);
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
                ServiceFormPage page = new ServiceFormPage(selectedItem);
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
                MessageBox.Show("Пожалуйста, выберите элемент для обновления.");
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
            var searchResult = (from serviceitem in Context.DB.Услуги.Local.ToList()
                                join priceitem in Context.DB.Стоимость_услуг.Local.ToList() on serviceitem.ID_услуги equals priceitem.ID_услуги
                                where serviceitem.Название.Contains(search) 
                                select new
                                {
                                    serviceitem.ID_услуги,
                                    serviceitem.Название,
                                    priceitem.Стоимость,
                                    priceitem.Скидка,
                                    priceitem.ID_стоимости_услуги
                                });
                        
            table.ItemsSource = searchResult.ToList();
        }
    }
}
