using Hospital.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hospital.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Password;

            // Проверка наличия введенных логина и пароля.
            if (string.IsNullOrWhiteSpace(login) && string.IsNullOrWhiteSpace(password))
            {
                System.Windows.MessageBox.Show("Логин и пароль не могут быть пустыми строками!");
                return;
            }
            if (string.IsNullOrWhiteSpace(login))
            {
                System.Windows.MessageBox.Show("Логин не может быть пустой строкой!");
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                System.Windows.MessageBox.Show("Пароль не может быть пустой строкой!");
                return;
            }
            var user = Context.DB.Пользователи.FirstOrDefault(x => x.Логин == login && x.Пароль == password && x.ID_роли == 2);
            var user1 = Context.DB.Пользователи.FirstOrDefault(x => x.Логин == login && x.Пароль == password && x.ID_роли == 1);
            if (user1 != null)
            {
                //CurrentUser.RoleID = 1;
                //CurrentUser.UserID = user1.UserID;
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Hide();
                }
                MainWindow menuWindow = new MainWindow();
                menuWindow.ShowDialog();
            }
            else if (user != null)
            {
                //CurrentUser.RoleID = 2;
                //CurrentUser.UserID = user.UserID;
                TimetablePage timetablePage = new TimetablePage();
                MainWindow menuWindow = new MainWindow
                {
                    Title = timetablePage.Title
                };
                menuWindow.MainFrame.Navigate(timetablePage);
                menuWindow.ShowDialog();
                var window = Window.GetWindow(this);
                window?.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            MainWindow m = new MainWindow();
            m.Show();
            Close();
        }
    }
}
