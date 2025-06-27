using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Dtos
{
    public record PublicUpdateModel
    {
        public record Request(string Sku, string Name, string Description, decimal Price, decimal stockQuantity);

        public record Response(Guid Id, string Sku, string Name, string Description, decimal Price, decimal stockQuantity);
    }
}
