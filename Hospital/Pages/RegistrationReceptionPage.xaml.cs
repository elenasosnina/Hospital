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
    /// Логика взаимодействия для RegistrationReceptionPage.xaml
    /// </summary>
    public partial class RegistrationReceptionPage : Page
    {
        public RegistrationReceptionPage()
        {
            InitializeComponent();
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

    }
}
