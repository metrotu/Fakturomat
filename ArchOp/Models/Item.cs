using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchOp.Models
{
    internal class Item(string name, string? description, double price, int quantity, double totalPrice)
    {
        public string Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public double Price { get; set; } = price;
        public int Quantity { get; set; } = quantity;
        public double TotalPrice { get; set; } = totalPrice;
        public double Amount() => Price * Quantity;
    }
}
