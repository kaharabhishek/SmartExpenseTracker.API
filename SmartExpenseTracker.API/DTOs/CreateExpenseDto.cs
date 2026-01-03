using System.ComponentModel.DataAnnotations;

namespace SmartExpenseTracker.API.DTOs
{
    public class CreateExpenseDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [Range(1, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        public string? Notes { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
