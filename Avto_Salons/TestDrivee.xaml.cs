using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    /// Логика взаимодействия для TestDrivee.xaml
    /// </summary>
    public partial class TestDrivee : Page
    {

        private SALON_AVTOEntities tesdr = new SALON_AVTOEntities();



        public TestDrivee()
        {
            InitializeComponent();
            avto.ItemsSource = tesdr.TestDrives.ToList();
            emp_id.ItemsSource = tesdr.Employees.ToList();
            emp_id.DisplayMemberPath = "surname"; 
            emp_id.SelectedValuePath = "Id_employee"; 

            cus_id.ItemsSource = tesdr.Customers.ToList();
            cus_id.DisplayMemberPath = "surname";
            cus_id.SelectedValuePath = "Id_customers";

            carr_id.ItemsSource = tesdr.Cars.ToList();
            carr_id.DisplayMemberPath = "year"; 
            carr_id.SelectedValuePath = "Id_cars";


            commentt.TextChanged += Commentt_TextChanged;

        }


        private void Commentt_TextChanged(object sender, TextChangedEventArgs e)
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



        private bool ValidateInputs()
        {
            if (emp_id.SelectedItem == null || cus_id.SelectedItem == null || carr_id.SelectedItem == null ||
                string.IsNullOrWhiteSpace(datee.Text) || string.IsNullOrWhiteSpace(commentt.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return false;
            }

            if (!DateTime.TryParse(datee.Text, out _))
            {
                MessageBox.Show("Некорректный формат даты. Пожалуйста, введите дату в формате: гггг-мм-дд.");
                return false;
            }

            return true;
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            TestDrives td = new TestDrives();
            td.employee_Id = ((Employees)emp_id.SelectedItem).Id_employee;
            td.customer_Id = ((Customers)cus_id.SelectedItem).Id_customers;
            td.car_Id = ((Cars)carr_id.SelectedItem).Id_cars;
            td.test_drive_date = DateTime.Parse(datee.Text);
            td.comments = commentt.Text;

            tesdr.TestDrives.Add(td);
            tesdr.SaveChanges();
            avto.ItemsSource = tesdr.TestDrives.ToList();
        }





        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(avto.SelectedItem != null)
            {
                tesdr.TestDrives.Remove(avto.SelectedItem as TestDrives);
                tesdr.SaveChanges();
                avto.ItemsSource = tesdr.TestDrives.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                if (!ValidateInputs())
                    return;

                TestDrives selectedTestDrive = (TestDrives)avto.SelectedItem;
                selectedTestDrive.employee_Id = ((Employees)emp_id.SelectedItem).Id_employee;
                selectedTestDrive.customer_Id = ((Customers)cus_id.SelectedItem).Id_customers;
                selectedTestDrive.car_Id = ((Cars)carr_id.SelectedItem).Id_cars;
                selectedTestDrive.test_drive_date = DateTime.Parse(datee.Text);
                selectedTestDrive.comments = commentt.Text;

                tesdr.SaveChanges();
                avto.ItemsSource = tesdr.TestDrives.ToList();
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
                TestDrives selectedTestDrive = avto.SelectedItem as TestDrives;

                emp_id.SelectedItem = tesdr.Employees.FirstOrDefault(emp => emp.Id_employee == selectedTestDrive.employee_Id);
                cus_id.SelectedItem = tesdr.Customers.FirstOrDefault(cust => cust.Id_customers == selectedTestDrive.customer_Id);
                carr_id.SelectedItem = tesdr.Cars.FirstOrDefault(car => car.Id_cars == selectedTestDrive.car_Id);

                datee.Text = selectedTestDrive.test_drive_date.ToString();
                commentt.Text = selectedTestDrive.comments;
            }
        }

    }
}
