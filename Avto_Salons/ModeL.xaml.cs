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
    /// Логика взаимодействия для ModeL.xaml
    /// </summary>
    public partial class ModeL : Page
    {

        private SALON_AVTOEntities moD = new SALON_AVTOEntities();

        public ModeL()
        {
            InitializeComponent();
            avto.ItemsSource = moD.Models.ToList();

            mod_n.TextChanged += (sender, e) => TextBox_TextChanged(sender, e, 30);
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

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(mod_n.Text))
            {
                MessageBox.Show("Пожалуйста, введите название модели.");
                return false;
            }

            if (!IsTextOnly(mod_n.Text))
            {
                MessageBox.Show("Название модели должно содержать только текст.");
                return false;
            }

            return true;
        }

        private bool IsTextOnly(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Zа-яА-Я\s]+$");
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            Models md = new Models();
            md.model_name = mod_n.Text;
            moD.Models.Add(md);
            moD.SaveChanges();
            avto.ItemsSource = moD.Models.ToList();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(avto.SelectedItem != null)
            {
                moD.Models.Remove(avto.SelectedItem as Models);
                moD.SaveChanges();
                avto.ItemsSource = moD.Models.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                if (!ValidateInputs())
                    return;

                Models selectedModel = avto.SelectedItem as Models;
                selectedModel.model_name = mod_n.Text;
                moD.SaveChanges();
                avto.ItemsSource = moD.Models.ToList();
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
                Models selectedModel = avto.SelectedItem as Models;
                mod_n.Text = selectedModel.model_name;
            }
        }
    }
}
