using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartExpenseTracker.API.Data;
using SmartExpenseTracker.API.DTOs;
using SmartExpenseTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/expense
        [HttpPost]
        public async Task<IActionResult> AddExpense(CreateExpenseDto dto)
        {
            // Validate User exists
            if (!await _context.Users.AnyAsync(u => u.UserId == dto.UserId))
                return BadRequest("Invalid UserId");

            // Validate Category exists
            if (!await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId))
                return BadRequest("Invalid CategoryId");

            var expense = new Expense
            {
                Title = dto.Title,
                Amount = dto.Amount,
                ExpenseDate = dto.ExpenseDate,
                Notes = dto.Notes,
                CategoryId = dto.CategoryId,
                UserId = dto.UserId
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return Ok(expense);
        }

        // GET: api/expense/user/1
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetExpensesByUser(int userId)
        {
            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .Include(e => e.Category)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();

            return Ok(expenses);
        }

        // GET: api/expense/filter
        [HttpGet("filter")]
        public async Task<IActionResult> FilterExpenses(
            int userId,
            int? categoryId,
            DateTime? startDate,
            DateTime? endDate)
        {
            var query = _context.Expenses
                .Where(e => e.UserId == userId)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(e => e.CategoryId == categoryId);

            if (startDate.HasValue)
                query = query.Where(e => e.ExpenseDate >= startDate);

            if (endDate.HasValue)
                query = query.Where(e => e.ExpenseDate <= endDate);

            var result = await query
                .Include(e => e.Category)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/expense/summary/monthly
        [HttpGet("summary/monthly")]
        public async Task<IActionResult> GetMonthlySummary(int userId, int year, int month)
        {
            var total = await _context.Expenses
                .Where(e => e.UserId == userId &&
                            e.ExpenseDate.Year == year &&
                            e.ExpenseDate.Month == month)
                .SumAsync(e => e.Amount);

            return Ok(new
            {
                UserId = userId,
                Year = year,
                Month = month,
                TotalExpense = total
            });
        }

        // GET: api/expense/summary/category
        [HttpGet("summary/category")]
        public async Task<IActionResult> GetCategorySummary(int userId)
        {
            var summary = await _context.Expenses
                .Where(e => e.UserId == userId)
                .GroupBy(e => e.Category.CategoryName)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .ToListAsync();

            return Ok(summary);
        }


    }
}
