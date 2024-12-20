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
    public partial class ServiceFormPage : Page
    {
        private dynamic _selectedItem = null;
        public ServiceFormPage()
        {
            InitializeComponent();
        }
        public ServiceFormPage(dynamic selectedBookItem)
        {
            InitializeComponent();
            _selectedItem = selectedBookItem;
            FillFormFields();

        }
        private void FillFormFields()
        {
            if (_selectedItem != null)
            {
                title.Text = _selectedItem.Название;
                cost.Text = _selectedItem.Стоимость.ToString();
                discount.Text = _selectedItem.Скидка.ToString();

            }

        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Услуги services;
                var existing = Context.DB.Услуги.FirstOrDefault(a => a.Название == title.Text.Trim());

                if (string.IsNullOrWhiteSpace(title.Text))
                {
                    throw new Exception("Поле 'Название услуги' не может быть пустым");
                }

                if (!int.TryParse(cost.Text.Trim(), out int costValue) || costValue < 0)
                {
                    throw new Exception("Поле 'Стоимость' должно содержать положительное целое число");
                }
                if (!int.TryParse(discount.Text.Trim(), out int discountValue) || discountValue < 0)
                {
                    throw new Exception("Поле 'Скидка' должно содержать положительное целое число");
                }

                if (existing != null)
                {
                    services = existing;
                }
                else
                {
                    Услуги service = new Услуги
                    {
                        Название = title.Text.Trim()
                    };

                    Стоимость_услуг costService = new Стоимость_услуг
                    {
                        Стоимость = costValue,
                        Скидка = discountValue,
                        ID_услуги = service.ID_услуги
                    };

                    if (_selectedItem != null)
                    {
                        service.ID_услуги = _selectedItem.ID_услуги;
                        costService.ID_стоимости_услуги = _selectedItem.ID_стоимости_услуги;
                        Update(service, costService);
                    }
                    else
                    {
                        Context.DB.Услуги.Add(service);
                        Context.DB.Стоимость_услуг.Add(costService);
                        Context.DB.SaveChanges();
                        MessageBox.Show("Запись успешно сохранена");
                        Window parentWindow = Window.GetWindow(this);
                        parentWindow?.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void Update(Услуги service, Стоимость_услуг costService)
        {
            try
            {

                var existingservice = Context.DB.Услуги.Find(service.ID_услуги);
                var existingcost = Context.DB.Стоимость_услуг.Find(costService.ID_стоимости_услуги);

                if (existingservice != null && existingcost != null)
                {

                    existingservice.Название = service.Название.Trim();
                    existingcost.Стоимость = costService.Стоимость;
                    existingcost.Скидка = costService.Скидка;

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
                    throw new Exception("Не удалось найти услугу для обновления");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
