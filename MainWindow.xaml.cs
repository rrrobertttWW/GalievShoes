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

namespace ГалиевЧудоОбувь
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            MainFrame.Navigated += MainFrame_Navigated;

            MainFrame.Navigate(new LoginPage());
            UpdateUserInfo();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            BtnBack.Visibility = MainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
            
            if (e.Content is LoginPage)
            {
                UserInfoTB.Visibility = Visibility.Collapsed;
                
                // Очищаем историю навигации при возврате на страницу логина
                while (MainFrame.CanGoBack)
                {
                    MainFrame.RemoveBackEntry();
                }
                BtnBack.Visibility = Visibility.Collapsed;
            }
            else
            {
                UserInfoTB.Visibility = Visibility.Visible;
            }
        }

        public void UpdateUserInfo()
        {
            UserInfoTB.Text = Manager.CurrentUser == null
                ? "Гость"
                : Manager.CurrentUser.UserSurname + " " + Manager.CurrentUser.UserName;

            if (Manager.IsGuest)
            {
                BtnCart.Visibility = Visibility.Collapsed;
                BtnOrders.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnCart.Visibility = Visibility.Visible;
                BtnOrders.Visibility = (Manager.IsManager || Manager.IsAdmin) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void BtnCart_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CartPage());
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdersPage());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }
    }
}
