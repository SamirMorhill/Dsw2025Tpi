using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderItem : EntityBase 
    {
        public OrderItem(int quantity, decimal unitPrice, Product product )
        {
            Quantity = quantity;
            UnitPrice = product.CurrentUnitPrice;
            ProductId = product.ProductId;

        }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => UnitPrice * Quantity;
        public Guid ProductId { get; private set; }


    }
}
