using Microsoft.EntityFrameworkCore;
using pet1_backend.Data;
using pet1_backend.Services;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection")
    ?? throw new InvalidOperationException("Connection string was not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
 {
    options.AddPolicy("_myAllowSpecificOrigins",
     builder => builder
     .WithOrigins("http://localhost:3000")
     .AllowAnyHeader()
     .AllowAnyMethod()
    );
 });

builder.Services.AddControllers();

builder.Services.AddScoped<IUserValidationService, UserValidationService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJWTTokenService, JwtTokenService>();

var app = builder.Build();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
