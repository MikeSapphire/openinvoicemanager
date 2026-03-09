namespace OpenInvoiceManager.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Konstruktor damit man nicht vergisst was zu setzen
        public Customer()
        {
            Name = "";
            Street = "";
            Zip = "";
            City = "";
            Email = "";
            Phone = "";
        }

        // Damit man den Kunden z.B. in einer ComboBox anzeigen kann
        public override string ToString()
        {
            return Name;
        }
    }
}
