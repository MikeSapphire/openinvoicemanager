namespace OpenInvoiceManager.Models
{
    // Firmendaten die in den Einstellungen gespeichert werden
    public class CompanySettings
    {
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string TaxNumber { get; set; }
        public string VatId { get; set; } // Umsatzsteuer-ID
        public string BankName { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public decimal DefaultTaxRate { get; set; }
        public int NextInvoiceNumber { get; set; } // wird hochgezählt

        public CompanySettings()
        {
            CompanyName = "";
            Street = "";
            Zip = "";
            City = "";
            Phone = "";
            Email = "";
            Website = "";
            TaxNumber = "";
            VatId = "";
            BankName = "";
            Iban = "";
            Bic = "";
            DefaultTaxRate = 19;
            NextInvoiceNumber = 1;
        }
    }
}
