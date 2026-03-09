namespace OpenInvoiceManager.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; } // z.B. "Stück", "Stunde", "Pauschal"

        public Article()
        {
            Name = "";
            Description = "";
            Unit = "Stück";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
