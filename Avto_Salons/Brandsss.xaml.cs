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
    /// Логика взаимодействия для Brandsss.xaml
    /// </summary>
    public partial class Brandsss : Page
    {

        private SALON_AVTOEntities br = new SALON_AVTOEntities();

        public Brandsss()
        {
            InitializeComponent();
            avto.ItemsSource = br.Brands.ToList();
        }




        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустое поле
            if (string.IsNullOrWhiteSpace(marka.Text))
            {
                MessageBox.Show("Введите название марки.");
                return;
            }

            // Проверка на соответствие регулярному выражению
            Regex regex = new Regex(@"^[A-Za-zА-Яа-я\s]+$");
            if (!regex.IsMatch(marka.Text))
            {
                MessageBox.Show("Название марки должно содержать только русские или английские буквы.");
                return;
            }


            if (marka.Text.Length > 20)
            {
                MessageBox.Show("Название марки не должно превышать 20 символов.");
                return;
            }

            Brands brands = new Brands();
            brands.brand_name = marka.Text;
            br.Brands.Add(brands);
            br.SaveChanges();
            avto.ItemsSource = br.Brands.ToList();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(avto.SelectedItem != null)
            {
                br.Brands.Remove(avto.SelectedItem as Brands);
                br.SaveChanges();
                avto.ItemsSource = br.Brands.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                // Проверка на пустое поле
                if (string.IsNullOrWhiteSpace(marka.Text))
                {
                    MessageBox.Show("Введите название марки.");
                    return;
                }

                // Проверка на соответствие регулярному выражению
                Regex regex = new Regex(@"^[A-Za-zА-Яа-я\s]+$");
                if (!regex.IsMatch(marka.Text))
                {
                    MessageBox.Show("Название марки должно содержать только русские или английские буквы.");
                    return;
                }

                if (marka.Text.Length > 20)
                {
                    MessageBox.Show("Название марки не должно превышать 20 символов.");
                    return;
                }

                Brands selectedBrand = avto.SelectedItem as Brands;
                selectedBrand.brand_name = marka.Text;
                br.SaveChanges();
                avto.ItemsSource = br.Brands.ToList();
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
                Brands selectedBrand = avto.SelectedItem as Brands;

                marka.Text = selectedBrand.brand_name;
            }
        }
    }
}
