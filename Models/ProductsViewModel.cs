namespace TL13Shop.Models
{
    public class ProductsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public string DefaultImgUrl { get; set; }
        public List<string> ProductImgs { get; set; }
    }
}
