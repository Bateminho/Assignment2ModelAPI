﻿using System.ComponentModel.DataAnnotations;
using ModelAPI.Models.ExpenseDTO;

namespace ModelAPI.Models.JobDTO;

public class JobWithExpensesDto
{
	public long Id { get; set; }

	[MaxLength(64)] public string? Customer { get; set; }

	public DateTimeOffset StartDate { get; set; }
	public int Days { get; set; }

	[MaxLength(128)] public string? Location { get; set; }

	[MaxLength(2000)] public string? Comments { get; set; }

	public List<ExpenseIdDto>? Expenses { get; set; }
}