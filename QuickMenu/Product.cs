namespace QuickMenu
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int Index { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageURL { get; set; }
        public string BackColor { get; set; }
        public string ForeColor { get; set; }
        public int FontSize { get; set; }
    }
}
