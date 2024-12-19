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
using Hospital;

namespace Hospital.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
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
                             serviceitem.Название,
                             priceitem.Стоимость,
                             priceitem.Скидка
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
    }
}
