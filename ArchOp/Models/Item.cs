using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchOp.Models
{
    internal class Item(string name, string? description, double price, double quantity, double totalPrice, double? vat)
    {
        public string Name { get; set; } = name;
        public string? Description { get; set; } = description;
        public double Price { get; set; } = price;
        public double Quantity { get; set; } = quantity;
        public double TotalPrice { get; set; } = totalPrice;
        public double? Vat { get; set; } = vat == null ? 0 : vat.Value;
        public double? DisplayVat { get=>vat; }
        public double ItemBrutto { get => Brutto(); }

        public double ItemNetto { get=> Netto(); }

        public double Amount() => Price * Quantity;
        public double Brutto() => Math.Round(Amount(), 2);
        public double Netto() => Vat == null ? Brutto() : Math.Round(Amount() * (1 - Vat.Value), 2);
    }
}
