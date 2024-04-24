using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
using System.Xml.Linq;

namespace Avto_Salons
{
    /// <summary>
    /// Логика взаимодействия для Userss.xaml
    /// </summary>
    public partial class Userss : Page
    {
        private SALON_AVTOEntities us = new SALON_AVTOEntities();

        public Userss()
        {
            InitializeComponent();

            avto.ItemsSource = us.Users.ToList();

            rolii.ItemsSource = us.Roles.ToList();
            rolii.DisplayMemberPath = "RoleName";
            rolii.SelectedValuePath = "ID_Role";

            // Привязываем событие PasswordChanged к методу TextBox_TextChanged
            sur.TextChanged += TextBox_TextChanged;
            pass.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Проверяем, не превышает ли текущая длина текста 30 символов
            if (textBox.Text.Length > 30)
            {
                // Если превышает, обрезаем текст до 30 символов
                textBox.Text = textBox.Text.Substring(0, 30);

                // Устанавливаем курсор в конец текста
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            // Проверяем, не превышает ли текущая длина текста 30 символов
            if (passwordBox.Password.Length > 30)
            {
                // Если превышает, обрезаем текст до 30 символов
                passwordBox.Password = passwordBox.Password.Substring(0, 30);
            }
        }

        private void avto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                var selectedUser = avto.SelectedItem as Users;

                // Установка значения в текстовое поле и пасвордбокс
                sur.Text = selectedUser.Username;
                pass.Password = selectedUser.Password_Users;
                rolii.SelectedItem = us.Roles.FirstOrDefault(role => role.ID_Role == selectedUser.Role_ID);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(sur.Text) || string.IsNullOrWhiteSpace(pass.Password) || rolii.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return false;
            }

            return true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            Users users = new Users();
            users.Username = sur.Text;
            users.Password_Users = pass.Password;
            users.Role_ID = ((Roles)rolii.SelectedItem).ID_Role;

            us.Users.Add(users);
            us.SaveChanges();
            avto.ItemsSource = us.Users.ToList();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                us.Users.Remove(avto.SelectedItem as Users); // Исправлено на удаление Users
                us.SaveChanges();
                avto.ItemsSource = us.Users.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                if (!ValidateInputs())
                    return;

                Users selectedUser = (Users)avto.SelectedItem;
                selectedUser.Role_ID = ((Roles)rolii.SelectedItem).ID_Role;
                selectedUser.Username = sur.Text;
                selectedUser.Password_Users = pass.Password;
                us.SaveChanges();
                avto.ItemsSource = us.Users.ToList();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Administrator adm = new Administrator();
            adm.Show();
            Window.GetWindow(this).Close();
        }
    }
}
