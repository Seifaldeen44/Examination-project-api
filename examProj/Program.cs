using examProj.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Add services FIRST
builder.Services.AddControllers();

builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ❌ ممنوع تضيف Services بعد السطر ده
var app = builder.Build();

// ✅ 2. Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
