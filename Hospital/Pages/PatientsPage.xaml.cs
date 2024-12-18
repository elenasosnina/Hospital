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
    /// Логика взаимодействия для PatientsPage.xaml
    /// </summary>
    public partial class PatientsPage : Page
    {
        public PatientsPage()
        {
            InitializeComponent();
        }

        private void Timetable_Click(object sender, RoutedEventArgs e)
        {
            TimetablePage table = new TimetablePage();
            MainWindow w = (MainWindow)Window.GetWindow(this);
            w.Title = table.Title;
            w.MainFrame.Navigate(table);
        }
    }
}
