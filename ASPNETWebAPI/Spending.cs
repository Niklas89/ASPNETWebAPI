using System.ComponentModel.DataAnnotations;

namespace ASPNETWebAPI
{
    public class Spending
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        // The amount can be a number with maximum two decimals
        [RegularExpression(@"^(\d{1,})+([\.\,]\d{1,2})?$")]
        public float Amount { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }

        public SpendingType SpendingType { get; set; }
        public int SpendingTypeId { get; set; }
    }
}
