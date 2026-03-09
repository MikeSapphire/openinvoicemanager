using OpenInvoiceManager.Database;
using OpenInvoiceManager.Interfaces;
using OpenInvoiceManager.Models;
using System.Windows;

namespace OpenInvoiceManager.Presenters
{
    public class CustomerPresenter
    {
        private ICustomerView _view;
        private CustomerRepository _repo;

        public CustomerPresenter(ICustomerView view)
        {
            _view = view;
            _repo = new CustomerRepository();
        }

        public void LoadCustomers()
        {
            var customers = _repo.GetAll();
            _view.ShowCustomers(customers);
        }

        public void Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadCustomers();
                return;
            }

            var result = _repo.Search(searchText);
            _view.ShowCustomers(result);
        }

        public void LoadCustomer(int id)
        {
            var customer = _repo.GetById(id);
            if (customer == null) return;

            _view.SelectedCustomerId = customer.Id;
            _view.CustomerName = customer.Name;
            _view.Street = customer.Street;
            _view.Zip = customer.Zip;
            _view.City = customer.City;
            _view.Email = customer.Email;
            _view.Phone = customer.Phone;
        }

        public void Save()
        {
            // einfache Validierung
            if (string.IsNullOrWhiteSpace(_view.CustomerName))
            {
                _view.ShowMessage("Bitte gib einen Namen ein.");
                return;
            }

            var customer = new Customer();
            customer.Id = _view.SelectedCustomerId;
            customer.Name = _view.CustomerName.Trim();
            customer.Street = _view.Street?.Trim() ?? "";
            customer.Zip = _view.Zip?.Trim() ?? "";
            customer.City = _view.City?.Trim() ?? "";
            customer.Email = _view.Email?.Trim() ?? "";
            customer.Phone = _view.Phone?.Trim() ?? "";

            _repo.Save(customer);

            _view.ShowMessage("Kunde gespeichert!");
            _view.ClearForm();
            LoadCustomers();
        }

        public void Delete(int id)
        {
            if (id == 0) return;

            var result = MessageBox.Show(
                "Kunden wirklich löschen?",
                "Löschen bestätigen",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _repo.Delete(id);
                _view.ShowMessage("Kunde gelöscht.");
                _view.ClearForm();
                LoadCustomers();
            }
        }
    }
}
