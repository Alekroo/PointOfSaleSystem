using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSaleSystem
{
    public class ProductDatabase
    {
        private static ProductDatabase instance;
        private ObservableCollection<ObservableCollection<Product>> productList;

        // Private constructor to prevent external instantiation
        private ProductDatabase()
        {
            productList = new ObservableCollection<ObservableCollection<Product>>();
            addFoodToProductList();
            addToysToProductList();
            addAccToProductList();
        }

        // Method to access the singleton instance
        public static ProductDatabase GetInstance()
        {
            if (instance == null)
            {
                instance = new ProductDatabase();
            }
            return instance;
        }


        // Method to get the list of projects
        public ObservableCollection<ObservableCollection<Product>> GetProjects()
        {
            return productList;
        }

        private void addFoodToProductList()
        {
            productList.Add(new ObservableCollection<Product>());
            productList[0].Add(new Product("Cat Snack", 1, 14.99f, "/Assets/catsnack.jpg"));
            productList[0].Add(new Product("Dog Yum!", 2, 9.99f, "/Assets/dogdinner.jpg"));
            productList[0].Add(new Product("Bird Food", 3, 3.99f, "/Assets/birdfood.jpg"));
        }

        private void addToysToProductList()
        {
            productList.Add(new ObservableCollection<Product>());
            productList[1].Add(new Product("Bone", 4, 1.99f, "/Assets/bone.jpg"));
            productList[1].Add(new Product("Mouse", 5, 5.0f, "/Assets/mouse.jpg"));
            productList[1].Add(new Product("Ball", 6, 10.0f, "/Assets/ball.jpg"));
        }        
        
        private void addAccToProductList()
        {
            productList.Add(new ObservableCollection<Product>());
            productList[2].Add(new Product("Bowl", 7, 1.0f, "/Assets/bowl.jpg"));
            productList[2].Add(new Product("Tower", 8, 29.89f, "/Assets/tower.jpg"));
            productList[2].Add(new Product("Bath", 9, 9.99f, "/Assets/bath.jpg"));
        }
    }
}
