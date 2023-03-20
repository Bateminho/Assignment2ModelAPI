using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using ModelAPI.Models;
using ModelAPI.Data;
using ModelAPI.Hubs;
using ModelAPI.Models.ExpenseDTO;

namespace ModelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHubContext<ExpenseHub> _hubContext;

        public ExpensesController(DataContext context, IHubContext<ExpenseHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;

            if (context.Models.Any() || context.Jobs.Any()) return;

            context.Models.Add(new Model());

            context.Jobs.Add(new Job());
        }



		[HttpPost]
        public async Task<ActionResult<List<ExpenseIdDto>>> PostExpense(ExpenseNoIdDto NewExpense)
        {
            var dbModel = _context.Models.Find(NewExpense.ModelId);
            if (dbModel == null) { return NotFound("Model not found"); }
            
            _context.Entry(dbModel)
           .Collection(m => m.Expenses)
           .Load();
            
            // add expense to the database and save changes
            _context.Expenses.Add(NewExpense.Adapt<Expense>());
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("expenseadded", NewExpense);

            var dbExpenses= await _context.Expenses.ToListAsync();

            return Accepted(dbExpenses.Adapt<List<ExpenseIdDto>>());

        }        
    }
}
