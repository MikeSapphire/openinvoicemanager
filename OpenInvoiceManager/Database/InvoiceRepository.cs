using Microsoft.Data.Sqlite;
using OpenInvoiceManager.Models;
using System;
using System.Collections.Generic;

namespace OpenInvoiceManager.Database
{
    public class InvoiceRepository
    {
        private CustomerRepository _customerRepo = new CustomerRepository();

        public List<Invoice> GetAll()
        {
            var list = new List<Invoice>();

            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                var cmd = new SqliteCommand("SELECT * FROM Invoices ORDER BY InvoiceDate DESC", con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var inv = ReadInvoice(reader);
                    list.Add(inv);
                }
            }

            // Kunden und Positionen laden
            foreach (var inv in list)
            {
                inv.Customer = _customerRepo.GetById(inv.CustomerId);
                inv.Items = GetItemsByInvoiceId(inv.Id);
            }

            return list;
        }

        public Invoice GetById(int id)
        {
            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                var cmd = new SqliteCommand("SELECT * FROM Invoices WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var inv = ReadInvoice(reader);
                    inv.Customer = _customerRepo.GetById(inv.CustomerId);
                    inv.Items = GetItemsByInvoiceId(inv.Id);
                    return inv;
                }
            }
            return null;
        }

        private Invoice ReadInvoice(SqliteDataReader reader)
        {
            var inv = new Invoice();
            inv.Id = reader.GetInt32(0);
            inv.InvoiceNumber = reader.GetString(1);
            inv.CustomerId = reader.GetInt32(2);
            inv.InvoiceDate = DateTime.Parse(reader.GetString(3));
            inv.DueDate = DateTime.Parse(reader.GetString(4));
            inv.Status = reader.GetString(5);
            inv.Notes = reader.IsDBNull(6) ? "" : reader.GetString(6);
            inv.TaxRate = (decimal)reader.GetDouble(7);
            return inv;
        }

        public List<InvoiceItem> GetItemsByInvoiceId(int invoiceId)
        {
            var list = new List<InvoiceItem>();

            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                var cmd = new SqliteCommand("SELECT * FROM InvoiceItems WHERE InvoiceId = @Id", con);
                cmd.Parameters.AddWithValue("@Id", invoiceId);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var item = new InvoiceItem();
                    item.Id = reader.GetInt32(0);
                    item.InvoiceId = reader.GetInt32(1);
                    item.Description = reader.GetString(2);
                    item.Quantity = (decimal)reader.GetDouble(3);
                    item.Unit = reader.IsDBNull(4) ? "Stück" : reader.GetString(4);
                    item.UnitPrice = (decimal)reader.GetDouble(5);
                    list.Add(item);
                }
            }

            return list;
        }

        public void Save(Invoice invoice)
        {
            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                if (invoice.Id == 0)
                {
                    var cmd = new SqliteCommand(@"
                        INSERT INTO Invoices (InvoiceNumber, CustomerId, InvoiceDate, DueDate, Status, Notes, TaxRate)
                        VALUES (@Nr, @CustId, @Date, @Due, @Status, @Notes, @Tax)", con);
                    cmd.Parameters.AddWithValue("@Nr", invoice.InvoiceNumber);
                    cmd.Parameters.AddWithValue("@CustId", invoice.CustomerId);
                    cmd.Parameters.AddWithValue("@Date", invoice.InvoiceDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Due", invoice.DueDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Status", invoice.Status);
                    cmd.Parameters.AddWithValue("@Notes", invoice.Notes ?? "");
                    cmd.Parameters.AddWithValue("@Tax", (double)invoice.TaxRate);
                    cmd.ExecuteNonQuery();

                    cmd = new SqliteCommand("SELECT last_insert_rowid()", con);
                    invoice.Id = (int)(long)cmd.ExecuteScalar();
                }
                else
                {
                    var cmd = new SqliteCommand(@"
                        UPDATE Invoices
                        SET InvoiceNumber=@Nr, CustomerId=@CustId, InvoiceDate=@Date,
                            DueDate=@Due, Status=@Status, Notes=@Notes, TaxRate=@Tax
                        WHERE Id=@Id", con);
                    cmd.Parameters.AddWithValue("@Nr", invoice.InvoiceNumber);
                    cmd.Parameters.AddWithValue("@CustId", invoice.CustomerId);
                    cmd.Parameters.AddWithValue("@Date", invoice.InvoiceDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Due", invoice.DueDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Status", invoice.Status);
                    cmd.Parameters.AddWithValue("@Notes", invoice.Notes ?? "");
                    cmd.Parameters.AddWithValue("@Tax", (double)invoice.TaxRate);
                    cmd.Parameters.AddWithValue("@Id", invoice.Id);
                    cmd.ExecuteNonQuery();

                    // alte Positionen löschen und neu anlegen
                    cmd = new SqliteCommand("DELETE FROM InvoiceItems WHERE InvoiceId = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", invoice.Id);
                    cmd.ExecuteNonQuery();
                }

                // Positionen speichern
                foreach (var item in invoice.Items)
                {
                    var cmd = new SqliteCommand(@"
                        INSERT INTO InvoiceItems (InvoiceId, Description, Quantity, Unit, UnitPrice)
                        VALUES (@InvId, @Desc, @Qty, @Unit, @Price)", con);
                    cmd.Parameters.AddWithValue("@InvId", invoice.Id);
                    cmd.Parameters.AddWithValue("@Desc", item.Description);
                    cmd.Parameters.AddWithValue("@Qty", (double)item.Quantity);
                    cmd.Parameters.AddWithValue("@Unit", item.Unit);
                    cmd.Parameters.AddWithValue("@Price", (double)item.UnitPrice);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                // erst Positionen löschen
                var cmd = new SqliteCommand("DELETE FROM InvoiceItems WHERE InvoiceId = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                cmd = new SqliteCommand("DELETE FROM Invoices WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
