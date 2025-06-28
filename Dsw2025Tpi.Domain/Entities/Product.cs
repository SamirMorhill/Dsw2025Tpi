using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public  class Product : EntityBase
    {

        public Product()
        {
            
        }

        public Product(string sku, string internalCode, string name, string description, decimal price, decimal stockQuantity)
        {
         
            Sku = sku;
            InternalCode = internalCode;
            Name = name;
            Description = description;
            CurrentUnitPrice = price;
            StockQuantity = (int)stockQuantity;
            IsActive = true;
            ProductId = Guid.NewGuid();
            

        }

        public string? Sku { get; set; }
        public string? InternalCode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal CurrentUnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public Guid ProductId { get; private set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

        public bool HasSufficientStock(int quantity)
        {
            return StockQuantity >= quantity;
        }

        public void DecreaseStock(int quantity)
        {
            if (!HasSufficientStock(quantity))
            {
                throw new InvalidOperationException($"Insuficiente stock para el producto {Name}");
            }
            StockQuantity -= quantity;
        }

        /*public void DecrementStock (int quantity)
        {
            if (StockQuantity < quantity )
            {
                throw new Exception($"Stock insuficiente para {Name}. Stock dispnible: {StockQuantity}, solicitado: {quantity}.");
            }
                StockQuantity -= quantity;
        }*/
    }
}
