using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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

namespace PointOfSaleSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<Product> cart = new ObservableCollection<Product>();
        private ObservableCollection<string> logList = new ObservableCollection<string>();
        private ObservableCollection<ObservableCollection<Product>> productList;
        private ProductDatabase mySingleton;

        public MainWindow()
        {
            InitializeComponent();
            updateList();
            setupProductList();
            LogListView.ItemsSource = logList;
        }

        private void setupProductList()
        {
            mySingleton = ProductDatabase.GetInstance();
            productList = mySingleton.GetProjects();
        }


        private void updateList()
        {
            myListView.ItemsSource = cart;
            cart.CollectionChanged += Items_CollectionChanged;


            calculateTotalSum();
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity")
            {
                calculateTotalSum();
            }
        }


        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (Product p in cart)
                {
                    p.PropertyChanged += Item_PropertyChanged;
                }
                calculateTotalSum();
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (Product p in cart)
                {
                    p.PropertyChanged += Item_PropertyChanged;
                }
                calculateTotalSum();
            }
        }

            private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create and open the product selection window
            ProductSelectionWindow productSelectionWindow = new ProductSelectionWindow(cart, totalSum.Text, logList);
            productSelectionWindow.ShowDialog();
        }



        private string PromptForNewName(string nameorid)
        {
            // You can implement a dialog to prompt the user for a new name here
            // For simplicity, we'll use a MessageBox for input in this example
            return Microsoft.VisualBasic.Interaction.InputBox($"Enter products {nameorid}:", "Add product", "");
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (myListView.SelectedItem != null)
            {
                // Remove the selected item from the ObservableCollection
                foreach(Product p in cart)
                {
                    if((Product)myListView.SelectedItem == p)
                    {
                        logList.Add($"All {p.Name}(ID: {p.Id}) removed from the cart");
                        cart.Remove(p);
                        calculateTotalSum();
                        return;
                    }
                }
            }
        }

        private void ChangeNameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (myListView.SelectedItem != null)
            {
                if (int.TryParse(PromptForNewNumber(), out int newNumber))
                {
                    foreach (Product p in cart)
                    {
                        if ((Product)myListView.SelectedItem == p)
                        {
                            p.Quantity = newNumber;
                            logList.Add($"Quantity of {p.Name}(ID: {p.Id}) has been changed to {newNumber}.");
                            calculateTotalSum();
                            return;
                        }
                    }
                }
            }
        }

        private string PromptForNewNumber()
        {
            // You can implement a dialog to prompt the user for a new name here
            // For simplicity, we'll use a MessageBox for input in this example
            return Microsoft.VisualBasic.Interaction.InputBox("How many?:", "Quantity", "");
        }

        private void calculateTotalSum()
        {
            float n = 0;
            foreach (Product product in cart)
            {
                n += product.Sum;
            }
            totalSum.Text = "Total sum: " + n.ToString();
        }


        private void AddProduct(object sender, RoutedEventArgs e, bool byId, string nameorid)
        {
            string searchValue = PromptForNewName(nameorid);
            if (!string.IsNullOrEmpty(searchValue))
            {
                foreach (ObservableCollection<Product> oc in productList)
                {
                    foreach (Product p in oc)
                    {
                        bool matchCondition = byId ? (p.Id.ToString() == searchValue) : (p.Name == searchValue);

                        if (matchCondition)
                        {
                            if (int.TryParse(PromptForNewNumber(), out int newNumber))
                            {
                                if (cart.Contains(p))
                                {
                                    p.Quantity += newNumber;
                                }
                                else
                                {
                                    p.Quantity = newNumber;
                                    cart.Add(p);
                                   
                                }
                                logList.Add($"{newNumber}x {p.Name}(ID: {p.Id}) added to the cart");
                                return;
                            }
                        }
                    }
                }
                MessageBox.Show($"Cannot find product with {(byId ? "ID" : "name")} '{searchValue}' in the system.");
            }
        }

        private void AddById_Click(object sender, RoutedEventArgs e)
        {
            AddProduct(sender, e, true, "ID");
        }

        private void AddByName_Click(object sender, RoutedEventArgs e)
        {
            AddProduct(sender, e, false, "name");
        }


        private void RemoveProduct(object sender, RoutedEventArgs e, bool byId, string nameorid)
        {
            string searchValue = PromptForNewName(nameorid);

            if (!string.IsNullOrEmpty(searchValue))
            {
                foreach (Product p in cart)
                {
                    bool matchCondition = byId ? (p.Id.ToString() == searchValue) : (p.Name == searchValue);

                    if (matchCondition)
                    {
                        if (int.TryParse(PromptForNewNumber(), out int newNumber))
                        {
                            if (p.Quantity - newNumber > 0)
                            {
                                p.Quantity -= newNumber;
                                logList.Add($"{newNumber}x {p.Name}(ID: {p.Id}) removed from the cart");
                            }
                            else
                            {
                                
                                cart.Remove(p);
                                logList.Add($"All {p.Name}(ID: {p.Id}) removed from the cart");
                            }
                            
                            return;
                        }
                    }
                }
                MessageBox.Show($"Cannot product with {(byId ? "ID" : "name")} '{searchValue}' is in your cart.");
            }
        }



        private void RemovedById_Click(object sender, RoutedEventArgs e)
        {
            RemoveProduct(sender, e, true, "ID");
        }

        private void RemovedByName_Click(object sender, RoutedEventArgs e)
        {
            RemoveProduct(sender, e, false, "Name");
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            var confirmationDialog = new ConfirmationDialog();
            confirmationDialog.Owner = this; // Set the owner to center it relative to the main window
            confirmationDialog.ShowDialog();

            if (confirmationDialog.IsDone)
            {
                Application.Current.Shutdown(); // Close the application
            }
        }
    }




}

