using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Order : EntityBase
    {
        public Order(DateTime date, string shippingAddress, string billingAddress, string note, decimal totalAmount, OrderStatus status)
        {
            Date = date;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Note = note;
            TotalAmount = totalAmount;
            Status = status;
        }

        public DateTime Date { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string Note { get; set; }
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

       
    }
}
