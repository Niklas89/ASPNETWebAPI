namespace ASPNETWebAPI
{
    public class User
    {
        public int Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public Currency Currency { get; set; }
        public int? CurrencyId { get; set; }
    }
}
