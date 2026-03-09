using Microsoft.Data.Sqlite;
using OpenInvoiceManager.Models;

namespace OpenInvoiceManager.Database
{
    // Einstellungen werden als Key-Value in der DB gespeichert
    public class SettingsRepository
    {
        public CompanySettings Load()
        {
            var s = new CompanySettings();

            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                var cmd = new SqliteCommand("SELECT Key, Value FROM Settings", con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string key = reader.GetString(0);
                    string val = reader.IsDBNull(1) ? "" : reader.GetString(1);

                    switch (key)
                    {
                        case "CompanyName": s.CompanyName = val; break;
                        case "Street": s.Street = val; break;
                        case "Zip": s.Zip = val; break;
                        case "City": s.City = val; break;
                        case "Phone": s.Phone = val; break;
                        case "Email": s.Email = val; break;
                        case "Website": s.Website = val; break;
                        case "TaxNumber": s.TaxNumber = val; break;
                        case "VatId": s.VatId = val; break;
                        case "BankName": s.BankName = val; break;
                        case "Iban": s.Iban = val; break;
                        case "Bic": s.Bic = val; break;
                        case "DefaultTaxRate":
                            if (decimal.TryParse(val, out decimal tax))
                                s.DefaultTaxRate = tax;
                            break;
                        case "NextInvoiceNumber":
                            if (int.TryParse(val, out int nr))
                                s.NextInvoiceNumber = nr;
                            break;
                    }
                }
            }

            return s;
        }

        public void Save(CompanySettings settings)
        {
            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                SaveValue(con, "CompanyName", settings.CompanyName);
                SaveValue(con, "Street", settings.Street);
                SaveValue(con, "Zip", settings.Zip);
                SaveValue(con, "City", settings.City);
                SaveValue(con, "Phone", settings.Phone);
                SaveValue(con, "Email", settings.Email);
                SaveValue(con, "Website", settings.Website);
                SaveValue(con, "TaxNumber", settings.TaxNumber);
                SaveValue(con, "VatId", settings.VatId);
                SaveValue(con, "BankName", settings.BankName);
                SaveValue(con, "Iban", settings.Iban);
                SaveValue(con, "Bic", settings.Bic);
                SaveValue(con, "DefaultTaxRate", settings.DefaultTaxRate.ToString());
                SaveValue(con, "NextInvoiceNumber", settings.NextInvoiceNumber.ToString());
            }
        }

        private void SaveValue(SqliteConnection con, string key, string value)
        {
            // INSERT OR REPLACE macht einen Update wenn Key schon existiert
            var cmd = new SqliteCommand(
                "INSERT OR REPLACE INTO Settings (Key, Value) VALUES (@Key, @Value)", con);
            cmd.Parameters.AddWithValue("@Key", key);
            cmd.Parameters.AddWithValue("@Value", value ?? "");
            cmd.ExecuteNonQuery();
        }
    }
}
