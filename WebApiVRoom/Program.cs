using Microsoft.EntityFrameworkCore;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.BLL.Infrastructure;
using Azure.Storage.Blobs;
using Algolia.Search.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddVRoomContext(connection);
builder.Services.AddUnitOfWorkService();
builder.Services.AddSingleton(x => {
    string? connectionString = builder.Configuration.GetConnectionString("AzureBlobConnectionString");
    return new BlobServiceClient(connectionString);
});
//string? AlgoliaAppId = builder.Configuration.GetConnectionString("AzureBlobConnectionString");
//string? AlgoliaKey = builder.Configuration.GetConnectionString("AzureBlobConnectionString");
//builder.Services.AddSingleton<ISearchClient>(sp =>
//    new SearchClient(AlgoliaAppId, AlgoliaKey));

string? blobStorageConnectionString = builder.Configuration["BlobStorage:ConnectionString"];
string? containerName = builder.Configuration["BlobStorage:ContainerName"];
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
builder.Services.AddScoped<IAlgoliaService,AlgoliaService>();
builder.Services.AddScoped<ILikesDislikesCVService, LikesDislikesCVService>();
builder.Services.AddScoped<ILikesDislikesVService, LikesDislikesVService>();
builder.Services.AddScoped<ILikesDislikesCPService, LikesDislikesCPService>();
builder.Services.AddScoped<ILikesDislikesAVService, LikesDislikesAVService>();
builder.Services.AddScoped<ILikesDislikesAPService, LikesDislikesAPService>();
builder.Services.AddScoped<ILikesDislikesPService, LikesDislikesPService>();


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
 app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader()
                           .AllowAnyMethod());
//app.UseCors(builder => builder.WithOrigins("http://localhost:54206")  //("http://localhost:54206")
//                            .AllowAnyHeader()
//                            .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
