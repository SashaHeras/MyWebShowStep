using System.ComponentModel.DataAnnotations;

namespace MyWebShowStep.Data
{
    public class Gpu
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Vendor { get; set; }

        public string GrapthChip { get; set; }

        public int RAM { get; set; }

        public string MemoryType { get; set; }
    }
}
