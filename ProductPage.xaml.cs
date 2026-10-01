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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            var products = Галиев_ботинкиEntities1.GetContext().Products.ToList();

            ProductListView.ItemsSource = products;
           
            FiltCB.SelectedIndex = 0;
            SortCB.SelectedIndex = 0;

            if (Manager.IsGuest)
            {
                SearchPanel.Visibility = Visibility.Collapsed;
                FilterPanel.Visibility = Visibility.Collapsed;
                SortPanel.Visibility = Visibility.Collapsed;
            }
            UpdateProducts();
        }
        private void UpdateProducts()
        {
            var currentProducts = Галиев_ботинкиEntities1.GetContext().Products.ToList();

            

            // filt
            if (FiltCB.SelectedIndex == 1)
                currentProducts = currentProducts.Where(p => p.CategoryID == 1).ToList();
            if (FiltCB.SelectedIndex == 2)
                currentProducts = currentProducts.Where(p => p.CategoryID == 2).ToList();
            if (FiltCB.SelectedIndex == 3)
                currentProducts = currentProducts.Where(p => p.CategoryID == 3).ToList();

            // search
            string searchText = (SearchTB.Text ?? "").ToLower();
            currentProducts = currentProducts
                .Where(p => (p.ProductName ?? "").ToLower().Contains(searchText))
                .ToList();

            // sort
            if (SortCB.SelectedIndex == 1)
                currentProducts = currentProducts.OrderByDescending(p => p.Price).ToList();
            if (SortCB.SelectedIndex == 2)
                currentProducts = currentProducts.OrderBy(p => p.Price).ToList();

            ProductListView.ItemsSource = currentProducts;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Manager.MainFrame.Navigate(new ProductPage());
        }
        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void FiltCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void SortCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.IsGuest)
            {
                MessageBox.Show("Для оформления заказа необходимо авторизоваться.");
                return;
            }

            Button btn = sender as Button;
            Stock selectedStock = btn.Tag as Stock;

            if (selectedStock == null)
            {
                MessageBox.Show("Пожалуйста, выберите размер перед добавлением в корзину.");
                return;
            }

            if (Manager.Cart.ContainsKey(selectedStock))
            {
                if (Manager.Cart[selectedStock] < selectedStock.Available)
                {
                    Manager.Cart[selectedStock]++;
                    MessageBox.Show("Количество увеличено.");
                }
                else
                {
                    MessageBox.Show("Достигнуто максимальное доступное количество на складе.");
                }
            }
            else
            {
                if (selectedStock.Available > 0)
                {
                    Manager.Cart.Add(selectedStock, 1);
                    MessageBox.Show("Товар добавлен в корзину.");
                }
                else
                {
                    MessageBox.Show("К сожалению, этого размера нет в наличии.");
                }
            }
        }
    }
}
