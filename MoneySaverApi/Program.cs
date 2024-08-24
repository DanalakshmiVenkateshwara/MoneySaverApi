using BusinessManagers.Interfaces;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories;
using DataAccess;
using BusinessManagers.Managers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.FileProviders;
using System.Data.Common;
using System.Data.SqlClient;
//using MoneySaver.Utilities.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Set environment variable for Firebase service account JSON
var credentialPath = Path.Combine(Directory.GetCurrentDirectory(),  "Configurations", "firebase-adminsdk.json");
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

// Initialize Firebase Admin SDK
FirebaseApp.Create(new AppOptions
{
    Credential = GoogleCredential.GetApplicationDefault()
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IRepository, RepositoryBase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddCors(o => o.AddPolicy("MoneySaverCors", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

// Configure static file serving
var fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files");
//builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(fileDirectory));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}
else
{
    app.UseCors("MoneySaverCors");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

// Serve static files from the "Files" directory
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(fileDirectory),
//    RequestPath = "/files"
//});

app.MapControllers();

app.Run();
