using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASPNETWebAPI.Models
{
    public class Currency
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [StringLength(3)]
        public string IsoCode { get; set; }

        [JsonIgnore]
        public List<User> Users { get; set; }
    }
}
