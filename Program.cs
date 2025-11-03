using Inventory.Api.Data;
using Microsoft.EntityFrameworkCore;
using Inventory.Api.Services;


var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddScoped<TransactionService>();



// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IMPORTANT: DbContext kaydı
var connStr = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connStr));

var app = builder.Build();

// Swagger'ı her ortamda aç (geliştirmede pratik)
app.UseSwagger();
app.UseSwaggerUI();

// İsteğe bağlı: root → swagger yönlendirme
app.MapGet("/", () => Results.Redirect("/swagger"));


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();