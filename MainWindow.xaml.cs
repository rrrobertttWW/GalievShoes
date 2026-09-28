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

            MainFrame.Navigate(new LoginPage());   // <-- было ProductPage
            UpdateUserInfo();
        }

        private void UpdateUserInfo()
        {
            UserInfoTB.Text = Manager.CurrentUser == null
                ? "Гость"
                : Manager.CurrentUser.UserSurname + " " + Manager.CurrentUser.UserName;
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentUser = null;
            UpdateUserInfo();
            MainFrame.Navigate(new LoginPage());
        }
       

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
        Manager.MainFrame.GoBack();
        }
    }
}
