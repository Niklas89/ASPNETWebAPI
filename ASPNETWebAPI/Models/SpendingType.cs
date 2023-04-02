using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASPNETWebAPI.Models
{
    public class SpendingType
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [JsonIgnore] // For Get spendings method, so we don't get circular error
        public List<Spending> Spendings { get; set; }
    }
}
