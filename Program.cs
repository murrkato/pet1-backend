using pet1_backend.Services;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

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
