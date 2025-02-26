using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using E_Commerce.DTO;

namespace E_Commerce.Models
{
    public class Order
    {
        [Key]
       public int Id { get; set; }
        public DateTime OrderDate { get; set; } 
        public double TotalPrice { get; set; }
        public string Status { get; set; }

        public string CustomerId { get; set; }

        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; }
        

    }
}
