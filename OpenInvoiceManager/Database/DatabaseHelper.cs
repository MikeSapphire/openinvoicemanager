using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace OpenInvoiceManager.Database
{
    // Hier wird die Datenbankverbindung verwaltet und die Tabellen angelegt
    public class DatabaseHelper
    {
        private static string _dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "OpenInvoiceManager",
            "database.db"
        );

        public static string ConnectionString
        {
            get { return $"Data Source={_dbPath}"; }
        }

        public static void Initialize()
        {
            // Ordner anlegen falls nicht vorhanden
            string folder = Path.GetDirectoryName(_dbPath);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            CreateTables();
        }

        private static void CreateTables()
        {
            using (var con = new SqliteConnection(ConnectionString))
            {
                con.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS Customers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Street TEXT,
                        Zip TEXT,
                        City TEXT,
                        Email TEXT,
                        Phone TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Articles (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Description TEXT,
                        Price REAL NOT NULL,
                        Unit TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Invoices (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        InvoiceNumber TEXT NOT NULL,
                        CustomerId INTEGER NOT NULL,
                        InvoiceDate TEXT NOT NULL,
                        DueDate TEXT NOT NULL,
                        Status TEXT NOT NULL,
                        Notes TEXT,
                        TaxRate REAL NOT NULL,
                        FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
                    );

                    CREATE TABLE IF NOT EXISTS InvoiceItems (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        InvoiceId INTEGER NOT NULL,
                        Description TEXT NOT NULL,
                        Quantity REAL NOT NULL,
                        Unit TEXT,
                        UnitPrice REAL NOT NULL,
                        FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id)
                    );

                    CREATE TABLE IF NOT EXISTS Settings (
                        Key TEXT PRIMARY KEY,
                        Value TEXT
                    );
                ";

                var cmd = new SqliteCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
