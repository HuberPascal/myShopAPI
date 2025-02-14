namespace myShopAPI.Models
{
    public class Product
    {
        public int? Id { get; set; } 
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public decimal ProductPrice { get; set; }
        public string? BasePrice { get; set; }
        public string ProductSpecification { get; set; }
        public int NumberOfRating { get; set; }
        public string ImgSrc { get; set; }
        public decimal Rating { get; set; }
    }
}
