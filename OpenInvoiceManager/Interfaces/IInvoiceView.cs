using System.Collections.Generic;
using OpenInvoiceManager.Models;

namespace OpenInvoiceManager.Interfaces
{
    public interface IInvoiceView
    {
        int SelectedCustomerId { get; set; }
        string InvoiceNumber { get; set; }
        string Status { get; set; }
        string Notes { get; set; }

        void ShowInvoices(List<Invoice> invoices);
        void ShowMessage(string message);
        void ClearForm();
    }
}
