using Microsoft.EntityFrameworkCore;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.BLL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddVRoomContext(connection);
builder.Services.AddUnitOfWorkService();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICountryService, CountryService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ILanguageService, LanguageService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//  CORS
// app.UseCors(builder => builder.AllowAnyOrigin());
app.UseCors(builder => builder.WithOrigins("http://localhost:54206")
                            .AllowAnyHeader()
                            .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
