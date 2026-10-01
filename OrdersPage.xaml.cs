using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ГалиевЧудоОбувь
{
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            UpdateOrders();
        }

        private void UpdateOrders()
        {
            try
            {
                var context = Галиев_ботинкиEntities1.GetContext();
                var ordersQuery = context.Orders.AsQueryable();
                
                if (Manager.IsUser)
                {
                    ordersQuery = ordersQuery.Where(o => o.UserID == Manager.CurrentUser.UserID);
                }

                var ordersList = ordersQuery.ToList().Select(o => new {
                    OrderID = o.OrderID,
                    OrderDate = o.OrderDate,
                    ClientName = o.Users.UserSurname + " " + o.Users.UserName + " " + o.Users.UserLastName,
                    Cost = o.Cost,
                    OriginalOrder = o
                }).ToList();
                
                OrdersGrid.ItemsSource = ordersList;

                if (Manager.IsManager || Manager.IsAdmin)
                {
                    BtnDeleteOrder.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заказов: " + ex.Message);
            }
        }

        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersGrid.SelectedItem != null)
            {
                dynamic selected = OrdersGrid.SelectedItem;
                Orders order = selected.OriginalOrder;
                
                try
                {
                    var context = Галиев_ботинкиEntities1.GetContext();
                    var items = context.OrderItems.Where(oi => oi.OrderID == order.OrderID).ToList();
                    OrderItemsGrid.ItemsSource = items;
                    OrderDetailsPanel.Visibility = Visibility.Visible;
                }
                catch
                {
                    OrderDetailsPanel.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                OrderDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem == null) return;
            
            if (MessageBox.Show("Вы точно хотите удалить этот заказ?", "Внимание", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    dynamic selected = OrdersGrid.SelectedItem;
                    Orders order = selected.OriginalOrder;
                    
                    var context = Галиев_ботинкиEntities1.GetContext();
                    var orderToDelete = context.Orders.Find(order.OrderID);
                    
                    if (orderToDelete != null)
                    {
                        var items = context.OrderItems.Where(oi => oi.OrderID == orderToDelete.OrderID).ToList();
                        
                        foreach (var item in items)
                        {
                            var stock = context.Stock.Find(item.StockID);
                            if (stock != null) stock.Available += item.Quantity;
                        }
                        
                        context.OrderItems.RemoveRange(items);
                        context.Orders.Remove(orderToDelete);
                        context.SaveChanges();
                        
                        UpdateOrders();
                        OrderDetailsPanel.Visibility = Visibility.Collapsed;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении заказа: " + ex.Message);
                }
            }
        }
    }
}
