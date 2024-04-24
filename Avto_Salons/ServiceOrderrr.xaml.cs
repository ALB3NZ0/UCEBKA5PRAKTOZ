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
    /// Логика взаимодействия для ServiceOrderrr.xaml
    /// </summary>
    public partial class ServiceOrderrr : Page
    {

        private SALON_AVTOEntities so = new SALON_AVTOEntities();


        public ServiceOrderrr()
        {
            InitializeComponent();
            avto.ItemsSource = so.ServiceOrders.ToList();

            emp_id.ItemsSource = so.Employees.ToList();
            emp_id.DisplayMemberPath = "surname";
            emp_id.SelectedValuePath = "Id_employee";

            cli_id.ItemsSource = so.Customers.ToList();
            cli_id.DisplayMemberPath = "surname";
            cli_id.SelectedValuePath = "Id_customers";

            paYY_id.ItemsSource = so.Payment_Method.ToList();
            paYY_id.DisplayMemberPath = "method";
            paYY_id.SelectedValuePath = "Id_payment";

            total.TextChanged += Total_TextChanged;

        }



        private void Total_TextChanged(object sender, TextChangedEventArgs e)
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




        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустые комбобоксы и текстовые поля
            if (emp_id.SelectedItem == null || cli_id.SelectedItem == null || paYY_id.SelectedItem == null ||
                string.IsNullOrWhiteSpace(daTa.Text) || string.IsNullOrWhiteSpace(total.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Проверка на корректность формата даты
            DateTime orderDate;
            if (!DateTime.TryParse(daTa.Text, out orderDate))
            {
                MessageBox.Show("Некорректный формат даты.");
                return;
            }

            // Проверка на корректность формата числа
            decimal totalAmount;
            if (!decimal.TryParse(total.Text, out totalAmount))
            {
                MessageBox.Show("Некорректный формат числа.");
                return;
            }

            // Создание объекта ServiceOrders и заполнение его данными
            ServiceOrders SO = new ServiceOrders();
            SO.employee_Id = ((Employees)emp_id.SelectedItem).Id_employee;
            SO.customer_Id = ((Customers)cli_id.SelectedItem).Id_customers;
            SO.payment_id = ((Payment_Method)paYY_id.SelectedItem).Id_payment;
            SO.order_date = orderDate;
            SO.total_amount = totalAmount;

            // Добавление объекта в базу данных и обновление списка
            so.ServiceOrders.Add(SO);
            so.SaveChanges();
            avto.ItemsSource = so.ServiceOrders.ToList();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(avto.SelectedItem != null)
            {
                so.ServiceOrders.Remove(avto.SelectedItem as ServiceOrders);
                so.SaveChanges();
                avto.ItemsSource = so.ServiceOrders.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                // Проверка на пустые комбобоксы и текстовые поля
                if (emp_id.SelectedItem == null || cli_id.SelectedItem == null || paYY_id.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(daTa.Text) || string.IsNullOrWhiteSpace(total.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                // Проверка на корректность формата даты
                DateTime orderDate;
                if (!DateTime.TryParse(daTa.Text, out orderDate))
                {
                    MessageBox.Show("Некорректный формат даты.");
                    return;
                }

                // Проверка на корректность формата числа
                decimal totalAmount;
                if (!decimal.TryParse(total.Text, out totalAmount))
                {
                    MessageBox.Show("Некорректный формат числа.");
                    return;
                }

                // Получение выбранной записи
                ServiceOrders selectedOrder = avto.SelectedItem as ServiceOrders;

                // Обновление данных выбранной записи
                selectedOrder.employee_Id = ((Employees)emp_id.SelectedItem).Id_employee;
                selectedOrder.customer_Id = ((Customers)cli_id.SelectedItem).Id_customers;
                selectedOrder.payment_id = ((Payment_Method)paYY_id.SelectedItem).Id_payment;
                selectedOrder.order_date = orderDate;
                selectedOrder.total_amount = totalAmount;

                // Сохранение изменений
                so.SaveChanges();

                // Обновление отображаемых данных в DataGrid
                avto.ItemsSource = so.ServiceOrders.ToList();
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
                ServiceOrders selectedOrder = avto.SelectedItem as ServiceOrders;

                // Заполнение комбо-боксов и текстовых полей данными выбранной записи
                emp_id.SelectedValue = selectedOrder.employee_Id;
                cli_id.SelectedValue = selectedOrder.customer_Id;
                paYY_id.SelectedValue = selectedOrder.payment_id;
                daTa.Text = selectedOrder.order_date.ToString();
                total.Text = selectedOrder.total_amount.ToString();
            }
        }
    }
}
