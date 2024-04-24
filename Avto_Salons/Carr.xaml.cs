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

namespace Avto_Salons
{
    /// <summary>
    /// Логика взаимодействия для Carr.xaml
    /// </summary>
    public partial class Carr : Page
    {

        private SALON_AVTOEntities carrrr = new SALON_AVTOEntities();

        public Carr()
        {
            InitializeComponent();
            avto.ItemsSource = carrrr.Cars.ToList();


            br_id.ItemsSource = carrrr.Brands.ToList();
            br_id.DisplayMemberPath = "brand_name";
            br_id.SelectedValuePath  = "Id_brands";

            con_id.ItemsSource = carrrr.Configurations.ToList();
            con_id.DisplayMemberPath = "name";
            con_id.SelectedValuePath = "Id_config";

            yea.PreviewTextInput += TextBox_PreviewTextInput;
            pr.PreviewTextInput += TextBox_PreviewTextInput;
            kol.PreviewTextInput += TextBox_PreviewTextInput;

        }


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Проверяем, является ли введенный символ числом
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Отменяем ввод, если символ не является числом
            }

            // Проверяем, что количество символов не превышает 20
            if (textBox.Text.Length >= 20)
            {
                e.Handled = true; // Отменяем ввод, если количество символов превышает 20
            }
        }







        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустоту выбранных элементов в комбобоксах
            if (br_id.SelectedItem == null || con_id.SelectedItem == null)
            {
                MessageBox.Show("Выберите марку автомобиля и конфигурацию.");
                return;
            }

            // Проверка на пустоту и валидацию введенных данных в текстовых полях
            if (string.IsNullOrWhiteSpace(yea.Text) || string.IsNullOrWhiteSpace(pr.Text) || string.IsNullOrWhiteSpace(kol.Text))
            {
                MessageBox.Show("Введите значения для всех полей.");
                return;
            }

            // Проверка, являются ли введенные данные в полях yea, pr, kol валидными числами
            if (!IsNumeric(yea.Text) || !IsDecimal(pr.Text) || !IsNumeric(kol.Text))
            {
                MessageBox.Show("Введите только цифры в поля год, цена и количество.");
                return;
            }

            Cars ca = new Cars();
            ca.brand_id = ((Brands)br_id.SelectedItem).Id_brands;
            ca.year = int.Parse(yea.Text);
            ca.price = decimal.Parse(pr.Text);
            ca.quantity_available = int.Parse(kol.Text);
            ca.configuration_id = ((Configurations)con_id.SelectedItem).Id_config;

            carrrr.Cars.Add(ca);
            carrrr.SaveChanges();
            avto.ItemsSource = carrrr.Cars.ToList();
        }

        // Метод для проверки, является ли строка числом
        private bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }

        // Метод для проверки, является ли строка десятичным числом
        private bool IsDecimal(string value)
        {
            return decimal.TryParse(value, out _);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(avto.SelectedItem != null)
            {
                carrrr.Cars.Remove(avto.SelectedItem as Cars);
                carrrr.SaveChanges();
                avto.ItemsSource = carrrr.Cars.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                // Проверка на пустоту выбранных элементов в комбобоксах
                if (br_id.SelectedItem == null || con_id.SelectedItem == null)
                {
                    MessageBox.Show("Выберите марку автомобиля и конфигурацию.");
                    return;
                }

                // Проверка на пустоту и валидацию введенных данных в текстовых полях
                if (string.IsNullOrWhiteSpace(yea.Text) || string.IsNullOrWhiteSpace(pr.Text) || string.IsNullOrWhiteSpace(kol.Text))
                {
                    MessageBox.Show("Введите значения для всех полей.");
                    return;
                }

                // Проверка, являются ли введенные данные в полях yea, pr, kol валидными числами
                if (!IsNumeric(yea.Text) || !IsDecimal(pr.Text) || !IsNumeric(kol.Text))
                {
                    MessageBox.Show("Введите только цифры в поля год, цена и количество.");
                    return;
                }

                Cars selectedCar = avto.SelectedItem as Cars;
                selectedCar.brand_id = ((Brands)br_id.SelectedItem).Id_brands;
                selectedCar.year = int.Parse(yea.Text);
                selectedCar.price = decimal.Parse(pr.Text);
                selectedCar.quantity_available = int.Parse(kol.Text);
                selectedCar.configuration_id = ((Configurations)con_id.SelectedItem).Id_config;
                carrrr.SaveChanges();
                avto.ItemsSource = carrrr.Cars.ToList();
            }
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            SaleManager sl = new SaleManager();
            sl.Show();
            Window.GetWindow(this).Close();
        }

        private void avto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, выбран ли элемент
            if (avto.SelectedItem != null)
            {
                // Приводим выбранный элемент к типу Cars
                Cars selectedCar = avto.SelectedItem as Cars;

                // Устанавливаем значения свойств выбранного элемента в соответствующие элементы управления
                br_id.SelectedItem = carrrr.Brands.FirstOrDefault(b => b.Id_brands == selectedCar.brand_id);
                yea.Text = selectedCar.year.ToString();
                pr.Text = selectedCar.price.ToString();
                kol.Text = selectedCar.quantity_available.ToString();
                con_id.SelectedItem = carrrr.Configurations.FirstOrDefault(c => c.Id_config == selectedCar.configuration_id);
            }
        }

    }
}
