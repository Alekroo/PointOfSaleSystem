using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PointOfSaleSystem
{
    /// <summary>
    /// Interaction logic for ProductSelectionWindow.xaml
    /// </summary>
    public partial class ProductSelectionWindow : Window
    {
        private ObservableCollection<ObservableCollection<Product>> productList;
        private ObservableCollection<Product> cart;
        private string totalSum;
        private ObservableCollection<string> logList;
        private ProductDatabase mySingleton;

        public ProductSelectionWindow(ObservableCollection<Product> cart, string totalSum, ObservableCollection<string> logList)
        {
            InitializeComponent();
            this.cart = cart;
            this.totalSum = totalSum;
            this.logList = logList;
            mySingleton = ProductDatabase.GetInstance();
            productList = mySingleton.GetProjects();
            Show_All();
        }


        public void getCart()
        {
            Console.WriteLine("got ya");
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Get the item container (parent of button) and find the quantity TextBox
                var stackPanel = button.Parent as StackPanel;
                if (stackPanel != null)
                {
                    var quantityTextBox = stackPanel.Children.OfType<TextBox>().FirstOrDefault();

                    if (quantityTextBox != null && int.TryParse(quantityTextBox.Text, out int quantity))
                    {
                        // Find the index (position) of the item in the ItemsControl
                        int index = ListViewProducts.Items.IndexOf(stackPanel.DataContext);

                        // Get the value of the item (you can use the DataContext)
                        Product value = (Product) stackPanel.DataContext;

                        foreach(Product p in cart)
                        {
                            if(p.Name == value.Name)
                            {
                                p.Quantity += quantity;
                                logList.Add($"{quantity}x {value.Name}(ID: {value.Id}) added to the cart");
                                calculateTotalSum();
                                return;
                            }
                        }
                        value.Quantity = quantity;
                        cart.Add(value);
                        calculateTotalSum();
                        logList.Add($"{quantity}x {value.Name}(ID: {value.Id}) added to the cart");
                    }
                    else
                    {
                        MessageBox.Show("Invalid input. Please enter a valid quantity.");
                    }
                }
            }
        }


        private void calculateTotalSum()
        {
            float n = 0;
            foreach (Product product in cart)
            {
                n += product.Sum;
            }
            totalSum = "Total sum: " + n.ToString();
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            Show_All();
        }

        private void Show_All()
        {
            List<Product> flattenedList = productList.SelectMany(innerList => innerList).ToList();
            ListViewProducts.ItemsSource = flattenedList;
        }

        private void Food_Click(object sender, RoutedEventArgs e)
        {
            ListViewProducts.ItemsSource = productList[0];
        }

        private void Toys_Click(object sender, RoutedEventArgs e)
        {
            ListViewProducts.ItemsSource = productList[1];
        }        
        private void Acc_Click(object sender, RoutedEventArgs e)
        {
            ListViewProducts.ItemsSource = productList[2];
        }
    }
}
