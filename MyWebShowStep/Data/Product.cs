using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebShowStep.Data
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped]
        public byte[] Image { get; set; }

        public int Price { get; set; }

        public int TypeId { get; set; }

        public int UserId { get; set; }
    }
}
