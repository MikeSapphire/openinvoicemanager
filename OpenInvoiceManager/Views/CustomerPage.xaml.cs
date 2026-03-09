using OpenInvoiceManager.Interfaces;
using OpenInvoiceManager.Models;
using OpenInvoiceManager.Presenters;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OpenInvoiceManager.Views
{
    public partial class CustomerPage : Page, ICustomerView
    {
        private CustomerPresenter _presenter;

        public CustomerPage()
        {
            InitializeComponent();
            _presenter = new CustomerPresenter(this);
            _presenter.LoadCustomers();
        }

        // ICustomerView Properties
        public string CustomerName
        {
            get { return TxtName.Text; }
            set { TxtName.Text = value; }
        }
        public string Street
        {
            get { return TxtStreet.Text; }
            set { TxtStreet.Text = value; }
        }
        public string Zip
        {
            get { return TxtZip.Text; }
            set { TxtZip.Text = value; }
        }
        public string City
        {
            get { return TxtCity.Text; }
            set { TxtCity.Text = value; }
        }
        public string Email
        {
            get { return TxtEmail.Text; }
            set { TxtEmail.Text = value; }
        }
        public string Phone
        {
            get { return TxtPhone.Text; }
            set { TxtPhone.Text = value; }
        }
        public int SelectedCustomerId { get; set; }

        public void ShowCustomers(List<Customer> customers)
        {
            LvKunden.ItemsSource = customers;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ClearForm()
        {
            SelectedCustomerId = 0;
            TxtName.Text = "";
            TxtStreet.Text = "";
            TxtZip.Text = "";
            TxtCity.Text = "";
            TxtEmail.Text = "";
            TxtPhone.Text = "";
            TxtFormTitle.Text = "Neuer Kunde";
            BtnLoeschen.IsEnabled = false;
            LvKunden.SelectedItem = null;
        }

        private void LvKunden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvKunden.SelectedItem is Customer selected)
            {
                _presenter.LoadCustomer(selected.Id);
                TxtFormTitle.Text = "Kunde bearbeiten";
                BtnLoeschen.IsEnabled = true;
            }
        }

        private void BtnNeu_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            TxtName.Focus();
        }

        private void BtnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            _presenter.Save();
        }

        private void BtnLoeschen_Click(object sender, RoutedEventArgs e)
        {
            _presenter.Delete(SelectedCustomerId);
        }

        private void BtnAbbrechen_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void BtnSuchen_Click(object sender, RoutedEventArgs e)
        {
            _presenter.Search(TxtSearch.Text);
        }

        private void TxtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                _presenter.Search(TxtSearch.Text);
        }
    }
}
