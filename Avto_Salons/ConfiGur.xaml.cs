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
    /// Логика взаимодействия для ConfiGur.xaml
    /// </summary>
    public partial class ConfiGur : Page
    {
        private SALON_AVTOEntities con = new SALON_AVTOEntities();

        public ConfiGur()
        {
            InitializeComponent();
            avto.ItemsSource = con.Configurations.ToList();

            komplekt.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e);
            opisanie.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e);
            cena.PreviewTextInput += cena_PreviewTextInput;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Проверяем, что вводимый символ является буквой
            if (!Regex.IsMatch(e.Text, @"^[А-Яа-яA-Za-z]+$"))
            {
                e.Handled = true; // Отменяем ввод, если введенный символ не является буквой
            }
        }

        private void cena_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли вводимый символ числом
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Отменяем ввод, если символ не является числом
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустоту полей
            if (string.IsNullOrWhiteSpace(komplekt.Text) || string.IsNullOrWhiteSpace(opisanie.Text) || string.IsNullOrWhiteSpace(cena.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Проверка на корректность формата числа
            decimal price;
            if (!decimal.TryParse(cena.Text, out price))
            {
                MessageBox.Show("Некорректный формат цены.");
                return;
            }

            // Создание объекта Configurations и заполнение его данными
            Configurations c = new Configurations();
            c.name = komplekt.Text;
            c.description = opisanie.Text;
            c.price = price;

            // Добавление объекта в базу данных и обновление списка
            con.Configurations.Add(c);
            con.SaveChanges();
            avto.ItemsSource = con.Configurations.ToList();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                con.Configurations.Remove(avto.SelectedItem as Configurations);
                con.SaveChanges();
                avto.ItemsSource = con.Configurations.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                // Проверка на пустоту полей
                if (string.IsNullOrWhiteSpace(komplekt.Text) || string.IsNullOrWhiteSpace(opisanie.Text) || string.IsNullOrWhiteSpace(cena.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Проверка на корректность формата числа
                decimal price;
                if (!decimal.TryParse(cena.Text, out price))
                {
                    MessageBox.Show("Некорректный формат цены.");
                    return;
                }

                // Создание объекта Configurations и заполнение его данными
                Configurations selectedConfiguration = avto.SelectedItem as Configurations;
                selectedConfiguration.name = komplekt.Text;
                selectedConfiguration.description = opisanie.Text;
                selectedConfiguration.price = price;

                // Сохранение изменений
                con.SaveChanges();

                // Обновление отображаемых данных в DataGrid
                avto.ItemsSource = con.Configurations.ToList();
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
            if (avto.SelectedItem != null)
            {
                Configurations selectedConfiguration = avto.SelectedItem as Configurations;
                komplekt.Text = selectedConfiguration.name;
                opisanie.Text = selectedConfiguration.description;
                cena.Text = selectedConfiguration.price.ToString();
            }
        }
    }
}
