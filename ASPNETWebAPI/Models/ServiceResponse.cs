using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETWebAPI.Models
{
    [NotMapped]
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
