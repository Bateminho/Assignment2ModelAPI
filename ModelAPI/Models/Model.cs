using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ModelAPI.Models.JobDTO;

namespace ModelAPI.Models
{
    public class Model
    { 
	    public Model()
	    {
		    FirstName = "FirstName";
		    LastName = "LastName";
		    Email = "Email";
		    PhoneNo = "PhoneNo";
		    AddresLine1 = "AddresLine1";
		    AddresLine2 = "AddresLine2";
		    Zip = "Zip";
		    City = "City";
		    BirthDay = DateTime.Now;
		    Height = 1.0;
		    ShoeSize = 1;
		    HairColor = "HairColor";
		    Comments = "Comments";
	    }
        
		public long Id { get; set; }
        [MaxLength(64)]
        public string? FirstName { get; set; }
        [MaxLength(32)]
        public string? LastName { get; set; }
        [MaxLength(254)]
        public string? Email { get; set; }
        [MaxLength(12)]
        public string? PhoneNo { get; set; }
        [MaxLength(64)]
        public string? AddresLine1 { get; set; }
        [MaxLength(64)]
        public string? AddresLine2 { get; set; }
        [MaxLength(9)]
        public string? Zip { get; set; }
        [MaxLength(64)]
        public string? City { get; set; }
        [Column(TypeName = "date")]
        public DateTime BirthDay { get; set; }
        public double Height { get; set; }
        public int ShoeSize { get; set; }
        [MaxLength(32)]
        public string? HairColor { get; set; }
        [MaxLength(1000)]
        public string? Comments { get; set; }
        public List<JobWithExpensesDto> Jobs { get; set; } = new();

        public List<Expense> Expenses { get; set; } = new();
    }

}
