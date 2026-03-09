using System.Collections.Generic;
using OpenInvoiceManager.Models;

namespace OpenInvoiceManager.Interfaces
{
    public interface ICustomerView
    {
        string CustomerName { get; set; }
        string Street { get; set; }
        string Zip { get; set; }
        string City { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        int SelectedCustomerId { get; set; }

        void ShowCustomers(List<Customer> customers);
        void ShowMessage(string message);
        void ClearForm();
    }
}
