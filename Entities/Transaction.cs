namespace TransactionApi.Entities
{
    public class Transaction
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime TransactionDateUTC { get; set; }
        public string Location { get; set; }
        public string TimeZone{ get; set; }


    }
}
