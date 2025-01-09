﻿
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.BLL.Infrastructure;
using Azure.Storage.Blobs;
using Algolia.Search.Clients;
using Microsoft.Extensions.Azure;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using WebApiVRoom;
using Microsoft.AspNetCore.Http.Features;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.DAL.Interfaces;
using WebApiVRoom.DAL.Repositories;
using WebApiVRoom.DAL.Interfaces;
using Google;
using WebApiVRoom.DAL.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
string? blobStorageConnectionString = builder.Configuration["BlobStorage:ConnectionString"];
string? containerName = builder.Configuration["BlobStorage:ContainerName"];

//builder.Services.AddVRoomContext(connection);
builder.Services.AddDbContext<VRoomContext>(options =>
    options.UseSqlServer(
       connection, b => b.MigrationsAssembly("WebApiVRoom.DAL")
    ));
builder.Services.AddUnitOfWorkService();

builder.Services.AddSingleton(x => {
    string? connectionString = builder.Configuration.GetConnectionString("AzureBlobConnectionString");
    return new BlobServiceClient(connectionString);
});
builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<PlayList, PlayListDTO>()
        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
        .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
        .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
        .ForMember(dest => dest.Access, opt => opt.MapFrom(src => src.Access))
        .ForMember(dest => dest.VideosId, opt => opt.MapFrom(src => src.PlayListVideos.Select(ch => ch.VideoId)));
});
builder.Services.AddSignalR();
builder.Services.AddScoped<IWebRTCSessionRepository, WebRTCSessionRepository>();
builder.Services.AddScoped<IWebRTCConnectionRepository, WebRTCConnectionRepository>();
builder.Services.AddScoped<IWebRTCService, WebRTCService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICountryService, CountryService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ILanguageService, LanguageService>();
builder.Services.AddTransient<IChannelSettingsService, ChannelSettingsService>();
builder.Services.AddTransient<IAnswerPostService, AnswerPostService>();
builder.Services.AddTransient<IAnswerVideoService, AnswerVideoService>();
//builder.Services.AddTransient<ICommentPostService, CommentPostService>();
//builder.Services.AddTransient<ICommentVideoService, CommentVideoService>();
builder.Services.AddTransient<IHistoryOfBrowsingService, HistoryOfBrowsingService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IPlayListService, PlayListService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<ISubscriptionService, SubscriptionService>();
builder.Services.AddTransient<ITagService, TagService>();
builder.Services.AddTransient<IVideoService, WebApiVRoom.BLL.Services.VideoService>();
if (string.IsNullOrEmpty(blobStorageConnectionString))
{
    throw new ArgumentNullException("ConnectionString", "Blob Storage connection string is not configured properly.");
}

//if (string.IsNullOrEmpty(blobStorageConnectionString))
//{
//    throw new ArgumentNullException("ConnectionString", "Blob Storage connection string is not configured properly.");
//}

builder.Services.AddSingleton(x => new BlobServiceClient(blobStorageConnectionString));

// ?????????? IBlobStorageService 
builder.Services.AddTransient<IBlobStorageService, BlobStorageService>(provider =>
{
    var blobServiceClient = provider.GetRequiredService<BlobServiceClient>();
    return new BlobStorageService(blobServiceClient, containerName);
});

// Scoped services
builder.Services.AddScoped<IPinnedVideoService, PinnedVideoService>();
builder.Services.AddScoped<IContentReportService, ContentReportService>();
builder.Services.AddScoped<IAdService, AdService>();
builder.Services.AddScoped<IAdminLogService, AdminLogService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILanguageService, LanguageService>(); 
builder.Services.AddScoped<IChannelSectionsService, ChannelSectionsService>();
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
builder.Services.AddScoped<IVideoService, WebApiVRoom.BLL.Services.VideoService>();
builder.Services.AddScoped<IAlgoliaService, AlgoliaService>();
builder.Services.AddScoped<ILikesDislikesCVService, LikesDislikesCVService>();
builder.Services.AddScoped<ILikesDislikesVService, LikesDislikesVService>();
builder.Services.AddScoped<ILikesDislikesCPService, LikesDislikesCPService>();
builder.Services.AddScoped<ILikesDislikesAVService, LikesDislikesAVService>();
builder.Services.AddScoped<ILikesDislikesAPService, LikesDislikesAPService>();
builder.Services.AddScoped<ILikesDislikesPService, LikesDislikesPService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddSingleton<LiveStreamingService>(provider =>new LiveStreamingService("AIzaSyDvXp8Wi0-Y6BPC55SDA953CFIid2g6TtY"));
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<IOptionsForPostService, OptionsForPostService>();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IVideoViewsService, VideoViewsService>();
builder.Services.AddScoped<ISubtitleService, SubtitleService>();



// Configure large file uploads
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 400_000_000;
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 400_000_000; 
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 400_000_000; 
});

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["BlobStorage:ConnectionString"]!);
    clientBuilder.AddQueueServiceClient(builder.Configuration["BlobStorage:ConnectionString"]!);
});

builder.Services.AddSignalR();

var app = builder.Build();
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2),
    ReceiveBufferSize = 4 * 1024
};
app.UseWebSockets(webSocketOptions);
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<VRoomContext>();
        context.Database.Migrate(); // Применение миграций
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка применения миграций: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.WithOrigins("http://localhost:3000", "https://9dda-176-98-71-120.ngrok-free.app")
// Enable CORS
app.UseCors(builder => builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        );

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"{header.Key}: {header.Value}");
    }
    await next.Invoke();
});
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/hub");
app.MapHub<WebRTCHub>("/webrtc");

app.Run();
