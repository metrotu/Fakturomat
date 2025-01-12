using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchOp.Models
{
    internal class Item
    {
        public string Name { get; set; }
        public string? Description { get; set; } 
        public double Price { get; set; }
        public double Quantity { get; set; }
        public double TotalPrice { get; set; } 
        public double Amount() => Price * Quantity;
    
        public Item(string name, string? description, double price, double quantity, double totalPrice)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            TotalPrice = totalPrice;
        }
    
    }
}
