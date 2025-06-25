using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderItem
    {
        public OrderItem(int quantity, int subTotal, decimal unitPrice)
        {
            Quantity = quantity;
            SubTotal = subTotal;
            UnitPrice = unitPrice;
        }
        public int Quantity { get; set; }
        public int SubTotal { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
