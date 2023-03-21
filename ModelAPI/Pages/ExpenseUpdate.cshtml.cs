using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ModelAPI.Pages;

public class ExpenseUpdateModel : PageModel
{
	private readonly ILogger<ExpenseUpdateModel> _logger;

	public ExpenseUpdateModel(ILogger<ExpenseUpdateModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}