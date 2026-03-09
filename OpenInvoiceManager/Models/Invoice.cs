using System;
using System.Collections.Generic;

namespace OpenInvoiceManager.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } // "Offen", "Bezahlt", "Ueberfaellig"
        public string Notes { get; set; }
        public List<InvoiceItem> Items { get; set; }

        // Steuersatz kommt aus den Einstellungen aber ich speicher ihn auch
        // in der Rechnung damit er sich nicht ändert wenn man die Einstellungen ändert
        public decimal TaxRate { get; set; }

        public decimal SubTotal
        {
            get
            {
                decimal sum = 0;
                foreach (var item in Items)
                    sum += item.Total;
                return sum;
            }
        }

        public decimal TaxAmount
        {
            get { return SubTotal * (TaxRate / 100); }
        }

        public decimal Total
        {
            get { return SubTotal + TaxAmount; }
        }

        public Invoice()
        {
            InvoiceNumber = "";
            Status = "Offen";
            Notes = "";
            Items = new List<InvoiceItem>();
            InvoiceDate = DateTime.Today;
            DueDate = DateTime.Today.AddDays(14);
            TaxRate = 19;
        }
    }
}
