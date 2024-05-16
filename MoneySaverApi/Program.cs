using BusinessManagers.Interfaces;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories;
using DataAccess;
using BusinessManagers.Managers;
using System.Data.Common;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    //app.UseSwagger();
    //app.UseSwaggerUI();

}
else
    app.UseCors("MoneySaverCors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
