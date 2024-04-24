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
    /// Логика взаимодействия для BrandsMOD.xaml
    /// </summary>
    public partial class BrandsMOD : Page
    {

        private SALON_AVTOEntities BMod = new SALON_AVTOEntities();

        public BrandsMOD()
        {
            InitializeComponent();
            avto.ItemsSource = BMod.BrandModels.ToList();

            br_id.ItemsSource = BMod.Brands.ToList();
            br_id.DisplayMemberPath = "brand_name";
            br_id.SelectedValuePath = "Id_brands";

            mod_id.ItemsSource = BMod.Models.ToList();
            mod_id.DisplayMemberPath = "model_name";
            mod_id.SelectedValuePath = "Id_model";

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбраны ли элементы в ComboBox'ах
            if (br_id.SelectedItem != null && mod_id.SelectedItem != null)
            {
                BrandModels bm = new BrandModels();
                bm.brand_id = ((Brands)br_id.SelectedItem).Id_brands;
                bm.model_id = ((Models)mod_id.SelectedItem).Id_model;

                BMod.BrandModels.Add(bm);
                BMod.SaveChanges();
                avto.ItemsSource = BMod.BrandModels.ToList();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите марку и модель.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                BMod.BrandModels.Remove(avto.SelectedItem as BrandModels);
                BMod.SaveChanges();
                avto.ItemsSource = BMod.BrandModels.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null && br_id.SelectedItem != null && mod_id.SelectedItem != null)
            {
                BrandModels selectedBrandModels = avto.SelectedItem as BrandModels;

                selectedBrandModels.brand_id = ((Brands)br_id.SelectedItem).Id_brands;
                selectedBrandModels.model_id = ((Models)mod_id.SelectedItem).Id_model;
                BMod.SaveChanges();
                avto.ItemsSource = BMod.BrandModels.ToList();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите марку, модель и элемент для обновления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                // Получаем выбранный объект BrandModels из DataGrid
                BrandModels selectedBrandModel = avto.SelectedItem as BrandModels;

                // Устанавливаем значение свойства brand_id выбранного элемента в ComboBox br_id
                br_id.SelectedValue = selectedBrandModel.brand_id;

                // Устанавливаем значение свойства model_id выбранного элемента в ComboBox mod_id
                mod_id.SelectedValue = selectedBrandModel.model_id;
            }
        }
    }
}
