using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SmartExpenseTracker.API.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        [Required]
        public string Title { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // 🔥 FIXED PROPERLY

        public DateTime ExpenseDate { get; set; }

        public string Notes { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
