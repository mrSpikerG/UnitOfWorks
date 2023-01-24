using System.ComponentModel.DataAnnotations;

namespace Domain {
    public class Product {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Cost { get; set; }
    }
}