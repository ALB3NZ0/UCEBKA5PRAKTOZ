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
using System.Xml.Linq;

namespace Avto_Salons
{
    /// <summary>
    /// Логика взаимодействия для SupplierCarr.xaml
    /// </summary>
    public partial class SupplierCarr : Page
    {

        private SALON_AVTOEntities supCarrr = new SALON_AVTOEntities();

        public SupplierCarr()
        {
            InitializeComponent();

            avto.ItemsSource = supCarrr.SupplierCars.ToList();
            sup_id.ItemsSource = supCarrr.Supplier.ToList();
            sup_id.DisplayMemberPath = "company_name";
            sup_id.SelectedValuePath = "Id_supplier";

            carr_id.ItemsSource = supCarrr.Cars.ToList();
            carr_id.DisplayMemberPath = "quantity_available";
            carr_id.SelectedValuePath = "Id_cars";
        }

        private bool ValidateInputs()
        {
            if (sup_id.SelectedItem == null || carr_id.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите поставщика и автомобиль.");
                return false;
            }

            return true;
        }



        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            SupplierCars supCarr = new SupplierCars();
            supCarr.supplier_Id = ((Supplier)sup_id.SelectedItem).Id_supplier;
            supCarr.car_Id = ((Cars)carr_id.SelectedItem).Id_cars;

            supCarrr.SupplierCars.Add(supCarr);
            supCarrr.SaveChanges();
            avto.ItemsSource = supCarrr.SupplierCars.ToList();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(avto.SelectedItem != null)
            {
                supCarrr.SupplierCars.Remove(avto.SelectedItem as SupplierCars);
                supCarrr.SaveChanges();
                avto.ItemsSource = supCarrr.SupplierCars.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null && sup_id.SelectedItem != null && carr_id.SelectedItem != null)
            {
                SupplierCars supCarr = avto.SelectedItem as SupplierCars;
                supCarr.supplier_Id = ((Supplier)sup_id.SelectedItem).Id_supplier;
                supCarr.car_Id = ((Cars)carr_id.SelectedItem).Id_cars;
                supCarrr.SaveChanges();
                avto.ItemsSource = supCarrr.SupplierCars.ToList();
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
                SupplierCars selectedSupplierCars = avto.SelectedItem as SupplierCars;

                sup_id.SelectedItem = supCarrr.Supplier.FirstOrDefault(emp => emp.Id_supplier == selectedSupplierCars.supplier_Id);
                carr_id.SelectedItem = supCarrr.Cars.FirstOrDefault(car => car.Id_cars == selectedSupplierCars.car_Id);
            }
        }
    }
}
