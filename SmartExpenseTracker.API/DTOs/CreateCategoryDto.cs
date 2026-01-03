using System.ComponentModel.DataAnnotations;

namespace SmartExpenseTracker.API.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
