using System.Windows;
using OpenInvoiceManager.Database;
using OpenInvoiceManager.Views;

namespace OpenInvoiceManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Datenbank beim Start initialisieren
            DatabaseHelper.Initialize();

            // Standardmäßig Kunden anzeigen
            MainFrame.Navigate(new CustomerPage());
        }

        private void BtnKunden_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CustomerPage());
        }

        private void BtnRechnungen_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new InvoicePage());
        }

        private void BtnArtikel_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ArticlePage());
        }

        private void BtnEinstellungen_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SettingsPage());
        }
    }
}
