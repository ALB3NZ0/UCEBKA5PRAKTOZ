using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Avto_Salons
{
    /// <summary>
    /// Логика взаимодействия для Customer.xaml
    /// </summary>
    public partial class Customer : Page
    {

        private SALON_AVTOEntities custom = new SALON_AVTOEntities();

        public Customer()
        {
            InitializeComponent();
            avto.ItemsSource = custom.Customers.ToList();

            namee.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e, 30);
            sur.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e, 30);
            mid.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e, 30);
            ph.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e, 30);
            em.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e, 30);

        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            SaleManager sm = new SaleManager();
            sm.Show();
            Window.GetWindow(this).Close();
        }






        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            string customerName = namee.Text;
            string customerSurname = sur.Text;
            string customerMiddleName = mid.Text;
            string customerPhoneNumber = ph.Text;
            string customerEmail = em.Text;

            Customers customer = new Customers();
            customer.name = customerName;
            customer.surname = customerSurname;
            customer.middle_name = customerMiddleName;
            customer.phone_number = customerPhoneNumber;
            customer.email = customerEmail;

            custom.Customers.Add(customer);
            custom.SaveChanges();
            avto.ItemsSource = custom.Customers.ToList();
        }





        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e, int maxLength)
        {
            TextBox textBox = sender as TextBox;

            // Проверяем, не превышает ли новая длина текста максимальное значение maxLength
            if (textBox.Text.Length + e.Text.Length > maxLength)
            {
                e.Handled = true; // Отменяем ввод, если новая длина превышает maxLength
            }
        }




        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(namee.Text) || string.IsNullOrEmpty(sur.Text) || string.IsNullOrEmpty(mid.Text) || string.IsNullOrEmpty(ph.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return false;
            }

            if (!IsTextOnly(namee.Text) || !IsTextOnly(sur.Text) || !IsTextOnly(mid.Text))
            {
                MessageBox.Show("Поля 'Имя', 'Фамилия' и 'Отчество' должны содержать только текст.");
                return false;
            }

            if (!Regex.IsMatch(ph.Text, @"^\+?[0-9]*$"))
            {
                MessageBox.Show("Номер телефона должен содержать только символ '+' и цифры.");
                return false;
            }

            return true;
        }

        private bool IsTextOnly(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Zа-яА-Я\s]+$");
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                custom.Customers.Remove(avto.SelectedItem as Customers);
                custom.SaveChanges();
                avto.ItemsSource = custom.Customers.ToList();
            }
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                if (!ValidateInputs())
                    return;

                string customerName = namee.Text;
                string customerSurname = sur.Text;
                string customerMiddleName = mid.Text;
                string customerPhoneNumber = ph.Text;
                string customerEmail = em.Text;

                // Получение выбранного клиента из списка
                Customers selectedCustomer = avto.SelectedItem as Customers;

                // Обновление данных клиента
                selectedCustomer.name = customerName;
                selectedCustomer.surname = customerSurname;
                selectedCustomer.middle_name = customerMiddleName;
                selectedCustomer.phone_number = customerPhoneNumber;
                selectedCustomer.email = customerEmail;

                // Сохранение изменений в базе данных
                custom.SaveChanges();
                avto.ItemsSource = custom.Customers.ToList();
            }
        }


        private void avto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                // Получаем выбранный элемент из списка клиентов
                Customers selectedCustomer = avto.SelectedItem as Customers;

                // Устанавливаем значения выбранного элемента в текстовых полях
                namee.Text = selectedCustomer.name;
                sur.Text = selectedCustomer.surname;
                mid.Text = selectedCustomer.middle_name;
                ph.Text = selectedCustomer.phone_number;
                em.Text = selectedCustomer.email;
            }
        }
    }
}