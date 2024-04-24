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
    /// Логика взаимодействия для ServiceOrdersCarr.xaml
    /// </summary>
    public partial class ServiceOrdersCarr : Page
    {

        private SALON_AVTOEntities SoC = new SALON_AVTOEntities();


        public ServiceOrdersCarr()
        {
            InitializeComponent();
            avto.ItemsSource = SoC.ServiceOrderCars.ToList();

            serO_Id.ItemsSource = SoC.ServiceOrders.ToList();
            serO_Id.DisplayMemberPath = "total_amount";
            serO_Id.SelectedValuePath = "Id_ServiceOrders";

            carr_id.ItemsSource = SoC.Cars.ToList();
            carr_id.DisplayMemberPath = "price";
            carr_id.SelectedValuePath = "Id_cars";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (serO_Id.SelectedItem == null || carr_id.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите заказ услуги и автомобиль для добавления.");
                return;
            }

            ServiceOrderCars svo = new ServiceOrderCars();
            svo.service_order_Id = ((ServiceOrders)serO_Id.SelectedItem).Id_ServiceOrders;
            svo.car_Id = ((Cars)carr_id.SelectedItem).Id_cars;

            SoC.ServiceOrderCars.Add(svo);
            SoC.SaveChanges();
            avto.ItemsSource = SoC.ServiceOrderCars.ToList();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null)
            {
                SoC.ServiceOrderCars.Remove(avto.SelectedItem as ServiceOrderCars);
                SoC.SaveChanges();
                avto.ItemsSource = SoC.ServiceOrderCars.ToList();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (avto.SelectedItem != null && serO_Id.SelectedItem != null && carr_id.SelectedItem != null)
            {
                ServiceOrderCars selectedServiceOrderCars = avto.SelectedItem as ServiceOrderCars;

                selectedServiceOrderCars.service_order_Id = ((ServiceOrders)serO_Id.SelectedItem).Id_ServiceOrders;
                selectedServiceOrderCars.car_Id = ((Cars)carr_id.SelectedItem).Id_cars;
                SoC.SaveChanges();
                avto.ItemsSource = SoC.ServiceOrderCars.ToList();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ услуги и автомобиль для обновления.");
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
                ServiceOrderCars selectedServiceOrderCars = avto.SelectedItem as ServiceOrderCars;


                serO_Id.SelectedValue = selectedServiceOrderCars.service_order_Id;

  
                carr_id.SelectedValue = selectedServiceOrderCars.car_Id;
            }
        }
    }
}
