using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для Cassa.xaml
    /// </summary>
    public partial class Cassa : Page
    {

        private Dictionary<string, (string clientInfo, string paymentMethod)> clientsDict = new Dictionary<string, (string clientInfo, string paymentMethod)>();

        private Dictionary<string, string> checksDict = new Dictionary<string, string>();

        private SALON_AVTOEntities casa = new SALON_AVTOEntities();

        public Cassa()
        {
            InitializeComponent();
            avto.SelectionChanged += avto_SelectionChanged;
            listbox.SelectionChanged += listbox_SelectionChanged;

            amount.PreviewTextInput += amount_PreviewTextInput;

            LoadSavedChecks();

            using (var context = new SALON_AVTOEntities())
            {
                var clientInformation = context.ClientInformation.ToList();
                avto.ItemsSource = clientInformation;
            }
        }


        private void LoadSavedChecks()
        {
            // Путь к рабочему столу
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Получаем все текстовые файлы с префиксом "Чек_"
            string[] checkFiles = Directory.GetFiles(desktopPath, "Чек_*.txt");

            foreach (string filePath in checkFiles)
            {
                // Получаем имя чека без расширения
                string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                // Добавляем пару (имя чека, путь к файлу) в словарь
                checksDict.Add(fileName, filePath);
                // Добавляем имя чека в ComboBox
                vivod_chekov.Items.Add(fileName);
            }
        }

        private void amount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный символ числом
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Отменяем ввод, если символ не является числом
            }
        }


        private void exit_Click(object sender, RoutedEventArgs e)
        {
            SaleManager sl = new SaleManager();
            sl.Show();
            Window.GetWindow(this).Close();
        }

        private void avto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listbox.SelectedIndex = -1;
        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                var selectedItem = avto.SelectedItem as ClientInformation;
                string clientInfo = $"{selectedItem.EmployeeSurname}, {selectedItem.EmployeePosition}, {selectedItem.EmployeeEmail}, {selectedItem.BrandName}, {selectedItem.ModelName}, {selectedItem.CarYear}, {selectedItem.CarPrice}, {selectedItem.ConfigurationName}, {selectedItem.ConfigurationPrice}, {selectedItem.CustomerSurname}";

                // Формируем ключ на основе фамилии сотрудника и способа оплаты
                string key = $"{selectedItem.EmployeeSurname}_{selectedItem.PaymentMethod}";
                if (!clientsDict.ContainsKey(key))
                {
                    clientsDict[key] = (clientInfo, selectedItem.PaymentMethod);
                }

                // Обновляем список в ListBox
                UpdateListBox();
            }
        }

        private void UpdateListBox()
        {
            listbox.Items.Clear();
            foreach (var key in clientsDict.Keys)
            {
                var clientInfo = clientsDict[key].clientInfo;
                var paymentMethod = clientsDict[key].paymentMethod;

                listbox.Items.Add("Информация о клиенте и автомобиле:");
                listbox.Items.Add(clientInfo);
                listbox.Items.Add($"Способ оплаты: {paymentMethod}");
                listbox.Items.Add(""); // Добавляем пустую строку для разделения клиентов
            }
        }

        private void minus_Click(object sender, RoutedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                listbox.Items.Remove(listbox.SelectedItem);
            }
        }

        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            avto.SelectedIndex = -1;
        }

        private void check_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что внесена сумма
            if (string.IsNullOrEmpty(amount.Text))
            {
                MessageBox.Show("Введите сумму внесения.");
                return;
            }

            // Проверяем, что введена корректная сумма
            if (!int.TryParse(amount.Text, out int vnesenoAmount))
            {
                MessageBox.Show("Введите корректную сумму внесения.");
                return;
            }

            // Проверяем длину введенного числа
            if (amount.Text.Length > 10) // Максимальная длина для int, включая знак минус
            {
                MessageBox.Show("Слишком длинное число. Максимальная длина - 10 символов.");
                return;
            }

            // Проверяем, что введенная сумма больше нуля
            if (vnesenoAmount <= 0)
            {
                MessageBox.Show("Сумма внесения должна быть больше нуля.");
                return;
            }

            // Вычисляем общую сумму заказа
            decimal totalAmount = 0;
            foreach (var key in clientsDict.Keys)
            {
                // Обращаемся к частям кортежа для получения информации о клиенте и автомобиле
                string[] parts = clientsDict[key].clientInfo.Split(new string[] { ", " }, StringSplitOptions.None);
                if (parts.Length >= 9)
                {
                    if (decimal.TryParse(parts[6], out decimal carPrice) && decimal.TryParse(parts[8], out decimal configPrice))
                    {
                        totalAmount += carPrice + configPrice;
                    }
                }
            }

            // Проверяем, что внесенная сумма больше или равна общей сумме заказа
            if (vnesenoAmount >= totalAmount)
            {
                // Формируем строку для записи в файл
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Информация о клиенте и автомобиле:");
                foreach (var key in clientsDict.Keys)
                {
                    // Добавляем информацию о клиенте и способе оплаты
                    sb.AppendLine(clientsDict[key].clientInfo);
                    sb.AppendLine($"Способ оплаты: {clientsDict[key].paymentMethod}");
                }
                sb.AppendLine($"Общая сумма заказа: {totalAmount}");
                sb.AppendLine($"Внесено: {vnesenoAmount}");
                sb.AppendLine($"Сдача: {vnesenoAmount - totalAmount}");

                // Формируем путь к файлу с уникальным именем на рабочем столе
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string fileName = $"Чек_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt"; // Уникальное имя файла на основе текущей даты и времени
                string filePath = System.IO.Path.Combine(desktopPath, fileName);

                // Записываем строку в текстовый файл
                File.WriteAllText(filePath, sb.ToString());

                MessageBox.Show("Чек успешно сохранен на рабочем столе.");

                // Добавляем пару (имя чека, путь к файлу) в словарь
                checksDict.Add(fileName, filePath);
                // Добавляем имя чека в ComboBox
                vivod_chekov.Items.Add(fileName);
            }
            else
            {
                MessageBox.Show("Внесенная сумма должна быть больше или равна общей сумме заказа.");
            }
        }

        private void vivod_chekov_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем выбран ли элемент в ComboBox
            if (vivod_chekov.SelectedItem != null)
            {
                // Получаем имя выбранного чека
                string selectedCheckName = vivod_chekov.SelectedItem.ToString();
                // Получаем путь к файлу чека из словаря
                string selectedCheckPath = checksDict[selectedCheckName];

                // Читаем содержимое файла и выводим в TextBox vivod
                string fileContent = File.ReadAllText(selectedCheckPath);
                vivod.Text = fileContent;
            }
        }
    }
}
