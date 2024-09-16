using Microsoft.EntityFrameworkCore;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.BLL.Infrastructure;
using AutoMapper;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Algolia.Search.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
string? blobStorageConnectionString = builder.Configuration["BlobStorage:ConnectionString"];
string? containerName = builder.Configuration["BlobStorage:ContainerName"];
string? AlgoliaAppId = builder.Configuration.GetConnectionString("AzureBlobConnectionString");
string? AlgoliaKey = builder.Configuration.GetConnectionString("AzureBlobConnectionString");
builder.Services.AddVRoomContext(connection);
builder.Services.AddUnitOfWorkService();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICountryService, CountryService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ILanguageService, LanguageService>();
builder.Services.AddTransient<IChannelSettingsService, ChannelSettingsService>();
builder.Services.AddTransient<IAnswerPostService, AnswerPostService>();
builder.Services.AddTransient<IAnswerVideoService, AnswerVideoService>();
builder.Services.AddTransient<ICommentPostService, CommentPostService>();
builder.Services.AddTransient<ICommentVideoService, CommentVideoService>();
builder.Services.AddTransient<IHistoryOfBrowsingService, HistoryOfBrowsingService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IPlayListService, PlayListService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<ISubscriptionService, SubscriptionService>();
builder.Services.AddTransient<ITagService, TagService>();
builder.Services.AddTransient<IVideoService, VideoService>();
builder.Services.AddScoped<IAlgoliaService, AlgoliaService>();
builder.Services.AddSingleton<ISearchClient>(sp =>
    new SearchClient(AlgoliaAppId, AlgoliaKey));
if (string.IsNullOrEmpty(blobStorageConnectionString))
{
    throw new ArgumentNullException("ConnectionString", "Blob Storage connection string is not configured properly.");
}

builder.Services.AddSingleton(x => new BlobServiceClient(blobStorageConnectionString));

// Реєстрація IBlobStorageService 
builder.Services.AddTransient<IBlobStorageService, BlobStorageService>(provider =>
{
    var blobServiceClient = provider.GetRequiredService<BlobServiceClient>();
    return new BlobStorageService(blobServiceClient, containerName);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq:K1SZFPTOtr:KBHBeksoGMGw==:blob"]!, preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq:K1SZFPTOtr:KBHBeksoGMGw==:queue"]!, preferMsi: true);
});

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
