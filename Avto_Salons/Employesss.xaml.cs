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
    /// Логика взаимодействия для Employesss.xaml
    /// </summary>
    public partial class Employesss : Page
    {
        private SALON_AVTOEntities emp = new SALON_AVTOEntities();


        public Employesss()
        {
            InitializeComponent();
            avto.ItemsSource = emp.Employees.ToList();

            rol_id.ItemsSource = emp.Roles.ToList();
            rol_id.DisplayMemberPath = "RoleName";
            rol_id.SelectedValuePath = "ID_Role";

            namename.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e, 30);
            familia.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e, 30);
            sta.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e, 30);
            num_ph.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e, 30);
            em.PreviewTextInput += (sender, e) => TextBox_PreviewTextInput(sender, e, 30);

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


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            Employees employees = new Employees();
            employees.name = namename.Text;
            employees.surname = familia.Text;
            employees.position = sta.Text;
            employees.phone_number = num_ph.Text;
            employees.email = em.Text;
            employees.Role_ID = ((Roles)rol_id.SelectedItem).ID_Role;
            employees.Password = parol.Password;

            emp.Employees.Add(employees);
            emp.SaveChanges();
            avto.ItemsSource = emp.Employees.ToList();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(namename.Text) || string.IsNullOrEmpty(familia.Text) || string.IsNullOrEmpty(sta.Text) || string.IsNullOrEmpty(num_ph.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return false;
            }

            if (!Regex.IsMatch(num_ph.Text, @"^\+?[0-9]*$"))
            {
                MessageBox.Show("Номер телефона должен содержать только символ '+' и цифры.");
                return false;
            }

            if (!IsTextOnly(namename.Text) || !IsTextOnly(familia.Text) || !IsTextOnly(sta.Text))
            {
                MessageBox.Show("Поля 'Имя', 'Фамилия' и 'Должность' должны содержать только текст.");
                return false;
            }

            return true;
        }

        private bool IsTextOnly(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Zа-яА-Я\s]+$");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                emp.Employees.Remove(avto.SelectedItem as Employees);
                emp.SaveChanges();
                avto.ItemsSource = emp.Employees.ToList();

            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                if (!ValidateInputs())
                    return;

                Employees selectedEmployee = (Employees)avto.SelectedItem;

                selectedEmployee.name = namename.Text;
                selectedEmployee.surname = familia.Text;
                selectedEmployee.position = sta.Text;
                selectedEmployee.phone_number = num_ph.Text;
                selectedEmployee.email = em.Text;
                selectedEmployee.Role_ID = ((Roles)rol_id.SelectedItem).ID_Role;
                selectedEmployee.Password = parol.Password;

                emp.SaveChanges();
                avto.ItemsSource = emp.Employees.ToList();
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
                Employees selectedEmployee = avto.SelectedItem as Employees;

                // Обновляем поля на форме данными из выбранного элемента
                namename.Text = selectedEmployee.name;
                familia.Text = selectedEmployee.surname;
                sta.Text = selectedEmployee.position;
                num_ph.Text = selectedEmployee.phone_number;
                em.Text = selectedEmployee.email;
                parol.Password = selectedEmployee.Password;

                // Не сохраняем изменения в базе данных здесь!
                // Только обновляем поля на форме
                rol_id.SelectedItem = emp.Roles.FirstOrDefault(role => role.ID_Role == selectedEmployee.Role_ID);
            }
        }
    }
}
