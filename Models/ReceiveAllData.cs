using System.Collections.Generic;

namespace ST10362208_CLDV6221_POE.REDO.Models
{
    public class ReceiveAllData
    {
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Cart> CartItems { get; set; }
        public int CartItemCount { get; set; }
        public Product NewProduct { get; set; }

        public decimal Subtotal { get; set; }
        public decimal VatAmount { get; set; }
        public decimal Total { get; set; }
    }
}
