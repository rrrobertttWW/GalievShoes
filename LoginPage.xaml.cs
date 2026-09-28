using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ГалиевЧудоОбувь
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text.Trim();

            if (string.IsNullOrEmpty(login))
            {
                ErrorTB.Text = "Введите логин";
                return;
            }

            try
            {
                var user = Галиев_ботинкиEntities.GetContext().Users
                    .FirstOrDefault(u => u.UserLogin == login);

                if (user == null)
                {
                    ErrorTB.Text = "Пользователь не найден";
                    return;
                }

                Manager.CurrentUser = user;
                Manager.MainFrame.Navigate(new ProductPage());
            }
            catch (System.Exception ex)
            {
                ErrorTB.Text = "Ошибка БД: " + ex.Message;
            }
        }

        private void GuestBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentUser = null; // гость
            Manager.MainFrame.Navigate(new ProductPage());
        }
    }
}