using Microsoft.Data.Sqlite;
using OpenInvoiceManager.Models;
using System.Collections.Generic;

namespace OpenInvoiceManager.Database
{
    public class ArticleRepository
    {
        public List<Article> GetAll()
        {
            var list = new List<Article>();

            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                var cmd = new SqliteCommand("SELECT * FROM Articles ORDER BY Name", con);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var a = new Article();
                    a.Id = reader.GetInt32(0);
                    a.Name = reader.GetString(1);
                    a.Description = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    a.Price = (decimal)reader.GetDouble(3);
                    a.Unit = reader.IsDBNull(4) ? "Stück" : reader.GetString(4);
                    list.Add(a);
                }
            }

            return list;
        }

        public void Save(Article article)
        {
            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                if (article.Id == 0)
                {
                    var cmd = new SqliteCommand(@"
                        INSERT INTO Articles (Name, Description, Price, Unit)
                        VALUES (@Name, @Description, @Price, @Unit)", con);
                    cmd.Parameters.AddWithValue("@Name", article.Name);
                    cmd.Parameters.AddWithValue("@Description", article.Description);
                    cmd.Parameters.AddWithValue("@Price", (double)article.Price);
                    cmd.Parameters.AddWithValue("@Unit", article.Unit);
                    cmd.ExecuteNonQuery();

                    cmd = new SqliteCommand("SELECT last_insert_rowid()", con);
                    article.Id = (int)(long)cmd.ExecuteScalar();
                }
                else
                {
                    var cmd = new SqliteCommand(@"
                        UPDATE Articles
                        SET Name=@Name, Description=@Description, Price=@Price, Unit=@Unit
                        WHERE Id=@Id", con);
                    cmd.Parameters.AddWithValue("@Name", article.Name);
                    cmd.Parameters.AddWithValue("@Description", article.Description);
                    cmd.Parameters.AddWithValue("@Price", (double)article.Price);
                    cmd.Parameters.AddWithValue("@Unit", article.Unit);
                    cmd.Parameters.AddWithValue("@Id", article.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var con = new SqliteConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();
                var cmd = new SqliteCommand("DELETE FROM Articles WHERE Id = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
