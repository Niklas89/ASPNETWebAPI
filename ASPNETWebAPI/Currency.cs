namespace ASPNETWebAPI
{
    public class Currency
    {
        public int Id { get; set; }

        public string Country { get; set; }

        public string IsoCode { get; set; }

        public List<User> Users { get; set; }
    }
}
