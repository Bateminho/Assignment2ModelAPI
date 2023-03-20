using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ModelAPI.Models.ExpenseDTO
{
    public class ExpenseNoIdDto
    {
        
        [Microsoft.Build.Framework.Required]
        public long ModelId { get; set; }
        [Microsoft.Build.Framework.Required]
        public long JobId { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public string? Text { get; set; }
        [Column(TypeName = "decimal(9,2)")]
        [Microsoft.Build.Framework.Required]
        public decimal amount { get; set; }
    }
}
