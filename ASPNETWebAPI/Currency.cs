using System.ComponentModel.DataAnnotations;

namespace ASPNETWebAPI
{
    public class Currency
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [StringLength(3)]
        public string IsoCode { get; set; }

        public List<User> Users { get; set; }
    }
}
