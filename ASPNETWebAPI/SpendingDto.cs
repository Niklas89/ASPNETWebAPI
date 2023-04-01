using System.ComponentModel.DataAnnotations;

namespace ASPNETWebAPI
{
    public class SpendingDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string UserFullName { get; set; }

        public DateTime Date { get; set; }

        public int SpendingTypeId { get; set; }
        public string SpendingTypeName { get; set; }

        // The amount can be a number with maximum two decimals
        [RegularExpression(@"^(\d{1,})+([\.\,]\d{1,2})?$")]
        public float Amount { get; set; }

        public string CurrencyName { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }
    }
}
