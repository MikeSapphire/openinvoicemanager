using Microsoft.Data.Sqlite;
using OpenInvoiceManager.Models;
using System.Collections.Generic;

namespace OpenInvoiceManager.Database
{
    public class CustomerRepository
    {
        public List<Customer> GetAll()
        {
            var list = new List<Customer>();

            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                var cmd = new SqliteCommand("SELECT * FROM Customers ORDER BY Name", con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var c = new Customer();
                    c.Id = reader.GetInt32(0);
                    c.Name = reader.GetString(1);
                    c.Street = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    c.Zip = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    c.City = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    c.Email = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    c.Phone = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    list.Add(c);
                }
            }

            return list;
        }

        public Customer GetById(int id)
        {
            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                var cmd = new SqliteCommand("SELECT * FROM Customers WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var c = new Customer();
                    c.Id = reader.GetInt32(0);
                    c.Name = reader.GetString(1);
                    c.Street = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    c.Zip = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    c.City = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    c.Email = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    c.Phone = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    return c;
                }
            }
            return null;
        }

        public void Save(Customer customer)
        {
            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                if (customer.Id == 0)
                {
                    // neuer Kunde
                    var cmd = new SqliteCommand(@"
                        INSERT INTO Customers (Name, Street, Zip, City, Email, Phone)
                        VALUES (@Name, @Street, @Zip, @City, @Email, @Phone)", con);

                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Street", customer.Street);
                    cmd.Parameters.AddWithValue("@Zip", customer.Zip);
                    cmd.Parameters.AddWithValue("@City", customer.City);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                    cmd.ExecuteNonQuery();

                    // ID zurücklesen
                    cmd = new SqliteCommand("SELECT last_insert_rowid()", con);
                    customer.Id = (int)(long)cmd.ExecuteScalar();
                }
                else
                {
                    // bestehenden Kunden updaten
                    var cmd = new SqliteCommand(@"
                        UPDATE Customers
                        SET Name=@Name, Street=@Street, Zip=@Zip, City=@City, Email=@Email, Phone=@Phone
                        WHERE Id=@Id", con);

                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Street", customer.Street);
                    cmd.Parameters.AddWithValue("@Zip", customer.Zip);
                    cmd.Parameters.AddWithValue("@City", customer.City);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                    cmd.Parameters.AddWithValue("@Id", customer.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                var cmd = new SqliteCommand("DELETE FROM Customers WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Customer> Search(string searchText)
        {
            var list = new List<Customer>();

            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                var cmd = new SqliteCommand(@"
                    SELECT * FROM Customers
                    WHERE Name LIKE @Search OR Email LIKE @Search OR City LIKE @Search
                    ORDER BY Name", con);
                cmd.Parameters.AddWithValue("@Search", "%" + searchText + "%");

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var c = new Customer();
                    c.Id = reader.GetInt32(0);
                    c.Name = reader.GetString(1);
                    c.Street = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    c.Zip = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    c.City = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    c.Email = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    c.Phone = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    list.Add(c);
                }
            }

            return list;
        }
    }
}
