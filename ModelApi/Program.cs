using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ModelAPI.Data;
using ModelAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// For use with SqlServer
//builder.Services.AddDbContext<DataContext>(options =>
//	options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext") ?? throw new InvalidOperationException("Connection string 'DataContext' not found.")));

builder.Services.AddDbContext<DataContext>(options =>
	options.UseInMemoryDatabase("InMemoryDb"));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

builder.Services.AddControllers();
// Add to fix circular reference problem with JSON serialization
builder.Services.AddControllers().AddJsonOptions(x =>
	x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(
		builder =>
		{
			builder.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.UseAuthorization();

// must be called before MapHub
app.UseCors();

app.MapRazorPages();
app.MapHub<ExpenseHub>("/expensehub");

app.Run();