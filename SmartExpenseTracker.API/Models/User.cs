using System.ComponentModel.DataAnnotations;

namespace SmartExpenseTracker.API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required] public string name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string passwordhash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
