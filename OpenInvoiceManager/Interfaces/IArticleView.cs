using System.Collections.Generic;
using OpenInvoiceManager.Models;

namespace OpenInvoiceManager.Interfaces
{
    public interface IArticleView
    {
        string ArticleName { get; set; }
        string Description { get; set; }
        decimal Price { get; set; }
        string Unit { get; set; }
        int SelectedArticleId { get; set; }

        void ShowArticles(List<Article> articles);
        void ShowMessage(string message);
        void ClearForm();
    }
}
