using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceFurniture.Domain
{
    public class Cart
    {
        public int Id { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        
        // Navigation Properties
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        
        // Calculated Properties
        public decimal TotalAmount => CartItems.Sum(item => item.Quantity * item.UnitPrice);
        public int TotalItems => CartItems.Sum(item => item.Quantity);
    }
} 