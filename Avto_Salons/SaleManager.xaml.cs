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
using System.Windows.Shapes;

namespace Avto_Salons
{
    /// <summary>
    /// Логика взаимодействия для SaleManager.xaml
    /// </summary>
    public partial class SaleManager : Window
    {
        public SaleManager()
        {
            InitializeComponent();
        }

        private void breend_Click(object sender, RoutedEventArgs e)
        {
            Brandsss brandsss = new Brandsss();
            this.Content = brandsss;
        }

        private void carr_Click(object sender, RoutedEventArgs e)
        {
            Carr carr = new Carr();
            this.Content = carr;
        }

        private void config_Click(object sender, RoutedEventArgs e)
        {
            ConfiGur confiGur = new ConfiGur();
            this.Content = confiGur;
        }

        private void cus_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer();
            this.Content = customer;
        }

        private void Pay_method_Click(object sender, RoutedEventArgs e)
        {
            Pay pay = new Pay();
            this.Content = pay;
        }

        private void modele_Click(object sender, RoutedEventArgs e)
        {
            ModeL modeL = new ModeL();
            this.Content = modeL;
        }

        private void testDrive_Click(object sender, RoutedEventArgs e)
        {
            TestDrivee testDrivee = new TestDrivee();
            this.Content = testDrivee;
        }

        private void brandMod_Click(object sender, RoutedEventArgs e)
        {
            BrandsMOD brandsMOD = new BrandsMOD();
            this.Content = brandsMOD;
        }

        private void ServiceO_Click(object sender, RoutedEventArgs e)
        {
            ServiceOrderrr serviceOrderrr = new ServiceOrderrr();
            this.Content = serviceOrderrr;
        }

        private void ServiceOC_Click(object sender, RoutedEventArgs e)
        {
            ServiceOrdersCarr s = new ServiceOrdersCarr();
            this.Content = s;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void chek_Click(object sender, RoutedEventArgs e)
        {
            Cassa cas = new Cassa();
            this.Content = cas;
        }
    }
}
