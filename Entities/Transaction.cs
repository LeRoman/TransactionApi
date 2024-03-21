namespace TransactionApi.Entities
{
    public class Transaction
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double Amount { get; set; }
        public DateTimeOffset TransactionDate { get; set; }
        public string Location { get; set; }

        public string TimeZone{ get; set; }


    }
}
