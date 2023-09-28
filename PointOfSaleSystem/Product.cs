using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSaleSystem
{
    public class Product : INotifyPropertyChanged
    {



        public string Name { get; }
        public string Image { get; }
        public string ProductInfo { get { return $"{Name} (ID: {Id})"; } }
        public int Id { get; }
        public float Price { get; }
        public string ProductPrice { get { return $"{Price}$"; } }



        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(Sum));
                }
            }
        }

        private float _sum;
        public float Sum
        {
            get { return Quantity * Price; }
            set
            {
                if (_sum != value)
                {
                    _sum = Quantity * Price;
                    OnPropertyChanged(nameof(Sum));
                }
            }
        }

        public Product(string name, int id, float price, string image)
        {
            Name = name;
            Price = price;
            Id = id;
            Image = image;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
