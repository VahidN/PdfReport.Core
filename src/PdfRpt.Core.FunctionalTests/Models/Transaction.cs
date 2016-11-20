using System;

namespace PdfRpt.Core.FunctionalTests.Models
{
    public class Transaction
    {
        public int Id { set; get; }
        public DateTime Date { set; get; }
        public string Description { set; get; }
        public float SalePrice { set; get; }
        public TransactionType Type { set; get; }
        public string SalesPerson { set; get; }
        public string Product { set; get; }

        public Transaction() { }
        public Transaction(DateTime date, string salesPerson, string product, float salePrice)
        {
            Date = date;
            SalesPerson = salesPerson;
            Product = product;
            SalePrice = salePrice;
        }
    }
}
