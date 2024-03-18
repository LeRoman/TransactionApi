namespace TransactionApi.Entities
{
    public class Transaction
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double Amount { get; set; }
        public string TransactionDate { get; set; }
        public string Location { get; set; }

    }
}
