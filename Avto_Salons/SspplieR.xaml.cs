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
using System.Text.RegularExpressions;

namespace Avto_Salons
{
    /// <summary>
    /// Логика взаимодействия для SspplieR.xaml
    /// </summary>
    public partial class SspplieR : Page
    {

        private SALON_AVTOEntities sup = new SALON_AVTOEntities();

        public SspplieR()
        {
            InitializeComponent();

            avto.ItemsSource = sup.Supplier.ToList();
            avto.SelectionChanged += avto_SelectionChanged;


            company_ad.TextChanged += Company_ad_TextChanged;
            company_na.TextChanged += Company_na_TextChanged;
            company_ph.TextChanged += Company_ph_TextChanged;


        }


        private void Company_ad_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Проверяем, не превышает ли текущая длина текста 50 символов
            if (textBox.Text.Length > 50)
            {
                // Если превышает, обрезаем текст до 50 символов
                textBox.Text = textBox.Text.Substring(0, 50);

                // Устанавливаем курсор в конец текста
                textBox.CaretIndex = 50;
            }
        }

        private void Company_na_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Проверяем, не превышает ли текущая длина текста 30 символов
            if (textBox.Text.Length > 30)
            {
                // Если превышает, обрезаем текст до 30 символов
                textBox.Text = textBox.Text.Substring(0, 30);

                // Устанавливаем курсор в конец текста
                textBox.CaretIndex = 30;
            }
        }

        private void Company_ph_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Проверяем, не превышает ли текущая длина текста 30 символов
            if (textBox.Text.Length > 30)
            {
                // Если превышает, обрезаем текст до 30 символов
                textBox.Text = textBox.Text.Substring(0, 30);

                // Устанавливаем курсор в конец текста
                textBox.CaretIndex = 30;
            }
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Administrator adm = new Administrator();
            adm.Show();
            Window.GetWindow(this).Close();
        }


        private bool IsPhoneNumberValid(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\+\d+$");
        }

        private bool IsTextOnly(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Zа-яА-Я\s]+$");
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(company_na.Text) ||
                string.IsNullOrWhiteSpace(company_ad.Text) ||
                string.IsNullOrWhiteSpace(company_ph.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return false;
            }

            if (!IsTextOnly(company_na.Text))
            {
                MessageBox.Show("Пожалуйста, введите только текст для названия компании.");
                return false;
            }

            if (!IsPhoneNumberValid(company_ph.Text))
            {
                MessageBox.Show("Некорректный формат номера телефона. Пожалуйста, введите номер в формате: +1234567890.");
                return false;
            }

            return true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            string companyName = company_na.Text;
            string companyAddress = company_ad.Text;
            string companyPhone = company_ph.Text;

            Supplier supplier = new Supplier();
            supplier.company_name = companyName;
            supplier.company_address = companyAddress;
            supplier.company_phone = companyPhone;

            sup.Supplier.Add(supplier);
            sup.SaveChanges();
            avto.ItemsSource = sup.Supplier.ToList();
        }



        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                sup.Supplier.Remove(avto.SelectedItem as Supplier);
                sup.SaveChanges();
                avto.ItemsSource = sup.Supplier.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                if (!ValidateInputs())
                    return;

                string companyName = company_na.Text;
                string companyAddress = company_ad.Text;
                string companyPhone = company_ph.Text;

                // Получение выбранного поставщика из списка
                Supplier selectedSupplier = avto.SelectedItem as Supplier;

                // Обновление данных поставщика
                selectedSupplier.company_name = companyName;
                selectedSupplier.company_address = companyAddress;
                selectedSupplier.company_phone = companyPhone;

                // Сохранение изменений в базе данных
                sup.SaveChanges();
                avto.ItemsSource = sup.Supplier.ToList();
            }
        }

        private void avto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {

                Supplier selectedSupplier = avto.SelectedItem as Supplier;


                company_na.Text = selectedSupplier.company_name;
                company_ad.Text = selectedSupplier.company_address;
                company_ph.Text = selectedSupplier.company_phone;
            }
        }
    }
}
