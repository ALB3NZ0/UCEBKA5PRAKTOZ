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
using System.Windows.Shapes;

namespace Avto_Salons
{
    /// <summary>
    /// Логика взаимодействия для Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        public Administrator()
        {
            InitializeComponent();
        }

        private void supplier_Click(object sender, RoutedEventArgs e)
        {
            SspplieR sup = new SspplieR();
            this.Content = sup;
        }

        private void role_Click(object sender, RoutedEventArgs e)
        {
            RolE rol = new RolE();
            this.Content = rol;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this).Close();
        }

        private void testDrive_Click(object sender, RoutedEventArgs e)
        {
            TestDrivee td = new TestDrivee();
            this.Content = td;
        }

        private void user_Click(object sender, RoutedEventArgs e)
        {
            Userss userss = new Userss();
            this.Content = userss;
        }

        private void SuppliercarS_Click(object sender, RoutedEventArgs e)
        {
            SupplierCarr supplierCarr = new SupplierCarr();
            this.Content = supplierCarr;
        }

        private void employ_Click(object sender, RoutedEventArgs e)
        {
            Employesss employesss = new Employesss();
            this.Content = employesss;
        }
    }
}
