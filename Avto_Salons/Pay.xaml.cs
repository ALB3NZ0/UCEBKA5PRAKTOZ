using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Pay.xaml
    /// </summary>
    public partial class Pay : Page
    {

        private SALON_AVTOEntities pa = new SALON_AVTOEntities();


        public Pay()
        {
            InitializeComponent();
            avto.ItemsSource = pa.Payment_Method.ToList();

            payM.TextChanged += (sender, e) => TextBox_TextChanged(sender, e, 30);
            opisanie.TextChanged += (sender, e) => TextBox_TextChanged(sender, e, 30);


        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e, int maxLength)
        {
            TextBox textBox = sender as TextBox;

            // Проверяем, не превышает ли текущая длина текста максимальное значение maxLength
            if (textBox.Text.Length > maxLength)
            {
                // Если превышает, обрезаем текст до maxLength символов
                textBox.Text = textBox.Text.Substring(0, maxLength);

                // Устанавливаем курсор в конец текста
                textBox.CaretIndex = maxLength;
            }
        }


        private bool IsTextOnly(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Zа-яА-Я\s]+$");
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(payM.Text) || string.IsNullOrWhiteSpace(opisanie.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return false;
            }

            if (!IsTextOnly(payM.Text) || !IsTextOnly(opisanie.Text))
            {
                MessageBox.Show("Пожалуйста, введите только текст.");
                return false;
            }

            return true;
        }



        private void avto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                // Приводим выбранный элемент к типу Payment_Method
                Payment_Method selectedPaymentMethod = avto.SelectedItem as Payment_Method;

                // Устанавливаем свойства выбранного элемента в TextBox
                payM.Text = selectedPaymentMethod.method;
                opisanie.Text = selectedPaymentMethod.description;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            Payment_Method pm = new Payment_Method();
            pm.method = payM.Text;
            pm.description = opisanie.Text;

            pa.Payment_Method.Add(pm);
            pa.SaveChanges();
            avto.ItemsSource = pa.Payment_Method.ToList();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(avto.SelectedItem != null)
            {
                pa.Payment_Method.Remove(avto.SelectedItem as Payment_Method);
                pa.SaveChanges();
                avto.ItemsSource = pa.Payment_Method.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                if (!ValidateInputs())
                    return;

                Payment_Method selectedPaymentMethod = avto.SelectedItem as Payment_Method;
                selectedPaymentMethod.method = payM.Text;
                selectedPaymentMethod.description = opisanie.Text;

                pa.SaveChanges();
                avto.ItemsSource = pa.Payment_Method.ToList();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            SaleManager sl = new SaleManager();
            sl.Show();
            Window.GetWindow(this).Close();
        }

        private void saVe_Click(object sender, RoutedEventArgs e)
        {
            List<Payment_Method> listpay = LabaConvert.DeserializeObject<List<Payment_Method>>();
            foreach(var pay in listpay)
            {
                pa.Payment_Method.Add(pay);
            }
            pa.SaveChanges();
            avto.ItemsSource = null;
            avto.ItemsSource = pa.Payment_Method.ToList();

        }
    }
}
