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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SALON_AVTOEntities avTO = new SALON_AVTOEntities();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void avtorization_Click(object sender, RoutedEventArgs e)
        {
            string log = Login.Text;
            string pas = Passwords.Password;

            var user = avTO.Users.FirstOrDefault(a => a.Username == log && a.Password_Users == pas);

            if (user != null)
            {
                switch (user.Role_ID)
                {
                    case 1:
                        SaleManager s = new SaleManager();
                        s.Show();
                        break;
                    case 2:
                        Administrator adm = new Administrator();
                        adm.Show();
                        break;
                    default:
                        MessageBox.Show("Не верная роль");
                        break;
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Не верный логин или пароль");
            }
        }

    }
}
