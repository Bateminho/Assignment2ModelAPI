using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Framework;

namespace ModelAPI.Models.ExpenseDTO;

public class ExpenseIdDto
{
	public long Id { get; set; }

	[Required] public long ModelId { get; set; }

	[Required] public long JobId { get; set; }

	[Column(TypeName = "date")] public DateTime Date { get; set; }

	public string? Text { get; set; }

	[Column(TypeName = "decimal(9,2)")]
	[Required]
	public decimal amount { get; set; }
}