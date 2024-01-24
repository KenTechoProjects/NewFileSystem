
namespace Domain.Entities
{
    public class RandomNumberViewModel
    {

   
            public int id { get; set; }
            public string answer { get; set; }
            public string question { get; set; }
            public int value { get; set; }
            public DateTime airdate { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public int category_id { get; set; }
            public int game_id { get; set; }
            public object invalid_count { get; set; }
            public Category category { get; set; }
        }

        public class Category
        {
            public int id { get; set; }
            public string title { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public int clues_count { get; set; }
        }
    public class TransactionViewModel
    {
        public DataViewModel data { get; set; }
        public string successMessage { get; set; }
        public string responseCode { get; set; }
        public bool isSuccessful { get; set; }
        public object error { get; set; }
    }

    public class DataViewModel
    {
        public string txnref { get; set; }
        public int merchantid { get; set; }
        public string channel { get; set; }
        public float amount { get; set; }
        public DateTime paymentDate { get; set; }
        public string paymentStatus { get; set; }
        public string furtherProcessed { get; set; }
        public string processDate { get; set; }
        public string merchantTxnref { get; set; }
        public decimal inAmount { get; set; }
        public string inCurrency { get; set; }
        public decimal rate { get; set; }
        public string redirectUrl { get; set; }
        public string transactionSource { get; set; }
        public string transactionChannel { get; set; }
    }

}




