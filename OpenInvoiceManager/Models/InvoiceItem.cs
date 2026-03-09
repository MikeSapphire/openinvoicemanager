namespace OpenInvoiceManager.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }

        // wird berechnet, nicht in DB gespeichert
        public decimal Total
        {
            get { return Quantity * UnitPrice; }
        }

        public InvoiceItem()
        {
            Description = "";
            Unit = "Stück";
            Quantity = 1;
        }
    }
}
