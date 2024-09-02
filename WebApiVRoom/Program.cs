using Microsoft.EntityFrameworkCore;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.BLL.Infrastructure;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddVRoomContext(connection);
builder.Services.AddUnitOfWorkService();
builder.Services.AddSingleton(x => {
    string connectionString = builder.Configuration["AzureBlob:ConnectionString"];
    return new BlobServiceClient(connectionString);
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IChannelSettingsService, ChannelSettingsService>();
builder.Services.AddScoped<IAnswerPostService, AnswerPostService>();
builder.Services.AddScoped<IAnswerVideoService, AnswerVideoService>();
builder.Services.AddScoped<ICommentPostService, CommentPostService>();
builder.Services.AddScoped<ICommentVideoService, CommentVideoService>();
builder.Services.AddScoped<IHistoryOfBrowsingService, HistoryOfBrowsingService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPlayListService, PlayListService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IVideoService, VideoService>();

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
 app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader()
                           .AllowAnyMethod());
//app.UseCors(builder => builder.WithOrigins("http://localhost:54206")  //("http://localhost:54206")
//                            .AllowAnyHeader()
//                            .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
