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
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.WebSockets;
using WebApiVRoom.DAL.EF;



var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(wwwrootPath))
{
    Directory.CreateDirectory(wwwrootPath);
}
var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
string? blobStorageConnectionString = builder.Configuration["BlobStorage:ConnectionString"];
string? containerName = builder.Configuration["BlobStorage:ContainerName"];

//builder.Services.AddVRoomContext(connection);
builder.Services.AddDbContext<VRoomContext>(options =>
    options.UseSqlServer(
       connection, b => b.MigrationsAssembly("WebApiVRoom.DAL")
    ));
builder.Services.AddUnitOfWorkService();
builder.Services.AddSingleton<IHLSService, HLSService>();


// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            //.WithOrigins("http://localhost:3000") // React app URL
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            //.AllowCredentials()
            ;
    });
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<PlayList, PlayListDTO>()
        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
        .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
        .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
        .ForMember(dest => dest.Access, opt => opt.MapFrom(src => src.Access))
        .ForMember(dest => dest.VideosId, opt => opt.MapFrom(src => src.PlayListVideo.Select(ch => ch.VideoId)));
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 400_000_000;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddScoped<IWebRTCSessionRepository, WebRTCSessionRepository>();
builder.Services.AddScoped<IWebRTCConnectionRepository, WebRTCConnectionRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAnswerVideoService, AnswerVideoService>();
builder.Services.AddTransient<ICommentPostService, CommentPostService>();
builder.Services.AddTransient<ICommentVideoService, CommentVideoService>();
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
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<IOptionsForPostService, OptionsForPostService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IVideoViewsService, VideoViewsService>();
builder.Services.AddScoped<ISubtitleService, SubtitleService>();



var app = builder.Build();

// Ensure streams directory exists
var streamsPath = Path.Combine(wwwrootPath, "streams");
if (!Directory.Exists(streamsPath))
{
    Directory.CreateDirectory(streamsPath);

    // Додаткові налаштування прав доступу для Linux
    if (OperatingSystem.IsWindows())
    {
        // Для Windows: встановлення прав доступу через ACL
        var directoryInfo = new DirectoryInfo(streamsPath);
        var accessControl = directoryInfo.GetAccessControl();
        accessControl.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(
            "Everyone",
            System.Security.AccessControl.FileSystemRights.FullControl,
            System.Security.AccessControl.InheritanceFlags.ContainerInherit | System.Security.AccessControl.InheritanceFlags.ObjectInherit,
            System.Security.AccessControl.PropagationFlags.None,
            System.Security.AccessControl.AccessControlType.Allow
        ));
        directoryInfo.SetAccessControl(accessControl);
    }
    else
    {
        // Для Linux: зміна прав доступу через chmod
        try
        {
            // Змінюємо права доступу до створеної папки (777 - повний доступ для всіх)
            System.Diagnostics.Process.Start("chmod", "777 " + streamsPath)?.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не вдалося змінити права доступу: {ex.Message}");
        }
    }
}


// Configure static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(wwwrootPath),
    RequestPath = "",
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<VRoomContext>();
        context.Database.Migrate(); // Ïðèìåíåíèå ìèãðàöèé
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Îøèáêà ïðèìåíåíèÿ ìèãðàöèé: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseWebSockets();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/hub");

app.Run();