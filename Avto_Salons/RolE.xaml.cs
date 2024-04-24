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
    /// Логика взаимодействия для RolE.xaml
    /// </summary>
    public partial class RolE : Page
    {

        private SALON_AVTOEntities rol = new SALON_AVTOEntities();

        public RolE()
        {
            InitializeComponent();

            avto.ItemsSource = rol.Roles.ToList();

            rol_NAme.TextChanged += (sender, e) => TextBox_TextChanged(sender, e, 30);


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
            if (string.IsNullOrWhiteSpace(rol_NAme.Text))
            {
                MessageBox.Show("Пожалуйста, введите название роли.");
                return false;
            }

            if (!IsTextOnly(rol_NAme.Text))
            {
                MessageBox.Show("Пожалуйста, введите только текст.");
                return false;
            }

            return true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            string roleName = rol_NAme.Text;

            Roles role = new Roles();
            role.RoleName = roleName;

            rol.Roles.Add(role);
            rol.SaveChanges();
            avto.ItemsSource = rol.Roles.ToList();
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                rol.Roles.Remove(avto.SelectedItem as Roles);
            }
            rol.SaveChanges();
            avto.ItemsSource = rol.Roles.ToList();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                if (!ValidateInputs())
                    return;

                string roleName = rol_NAme.Text;

                // Получение выбранной роли из списка
                Roles selectedRole = avto.SelectedItem as Roles;

                // Обновление данных роли
                selectedRole.RoleName = roleName;

                // Сохранение изменений в базе данных
                rol.SaveChanges();
                avto.ItemsSource = rol.Roles.ToList();
            }
        }


        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Administrator adm = new Administrator();
            adm.Show();
            Window.GetWindow(this).Close();
        }






        private void avto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                var selectedRole = avto.SelectedItem as Roles;

                if (selectedRole != null)
                {
                    // Проверяем, что выбранная роль действительно существует
                    rol_NAme.Text = selectedRole.RoleName;
                }
            }
        }

        private void saVe_Click(object sender, RoutedEventArgs e)
        {
            List<Roles> listrole = LabaConvert.DeserializeObject<List<Roles>>();
            foreach (var rols in listrole)
            {
                rol.Roles.Add(rols);
            }
            rol.SaveChanges();
            avto.ItemsSource = null;
            avto.ItemsSource = rol.Roles.ToList();
        }
    }
}
