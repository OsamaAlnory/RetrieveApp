using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements
{
    public class Product
    {
        private string name;
        private int amount;
        private float price;
        private float originalPrice;
        private DateTime take;
        private ImageSource image;

        public Product(string name, int amount, float price, 
            float originalPrice, DateTime take, ImageSource image)
        {
            this.name = name;
            this.amount = amount;
            this.price = price;
            this.originalPrice = originalPrice;
            this.take = take;
            this.image = image;
        }
        public string getName()
        {
            return name;
        }
        public int getAmount()
        {
            return amount;
        }
        public float getPrice()
        {
            return price;
        }
        public float getOriginalPrice()
        {
            return originalPrice;
        }
        public DateTime getDate()
        {
            return take;
        }
        public ImageSource getImage()
        {
            return image;
        }

    }
}
