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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
string? blobStorageConnectionString = builder.Configuration["BlobStorage:ConnectionString"];
string? containerName = builder.Configuration["BlobStorage:ContainerName"];
//string? AlgoliaAppId = builder.Configuration.GetConnectionString("AzureBlobConnectionString");
//string? AlgoliaKey = builder.Configuration.GetConnectionString("AzureBlobConnectionString");
builder.Services.AddVRoomContext(connection);
builder.Services.AddUnitOfWorkService();
builder.Services.AddSingleton(x => {
    string? connectionString = builder.Configuration.GetConnectionString("AzureBlobConnectionString");
    return new BlobServiceClient(connectionString);
});

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
builder.Services.AddTransient<IVideoService, VideoService>();
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
builder.Services.AddScoped<IAlgoliaService, AlgoliaService>();
builder.Services.AddScoped<ILikesDislikesCVService, LikesDislikesCVService>();
builder.Services.AddScoped<ILikesDislikesVService, LikesDislikesVService>();
builder.Services.AddScoped<ILikesDislikesCPService, LikesDislikesCPService>();
builder.Services.AddScoped<ILikesDislikesAVService, LikesDislikesAVService>();
builder.Services.AddScoped<ILikesDislikesAPService, LikesDislikesAPService>();
builder.Services.AddScoped<ILikesDislikesPService, LikesDislikesPService>();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors(builder => builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/hub");

app.Run();
