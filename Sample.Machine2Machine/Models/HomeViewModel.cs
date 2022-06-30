using System.ComponentModel.DataAnnotations;

namespace Sample.Machine2Machine.Models
{
    public class HomeViewModel
    {
        public bool isLogged { get; set; } = false;
        public string? token { get; set; }
        [Required]
        public string? client_id { get; set; }
        [Required]
        public string? client_secret { get; set; }
    }
}
