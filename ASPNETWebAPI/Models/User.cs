using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASPNETWebAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [JsonIgnore] // For Get spendings method, so we don't get circular error
        public List<Spending> Spendings { get; set; }

        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }
    }
}
