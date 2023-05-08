using EfCoreRepository;
using Microsoft.Data.SqlClient;
using RepairFirm.Shared.Models;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddControllers();

var connection = builder.Configuration.GetSection("ConnectionStrings");
builder.Services.AddSingleton<IDbConnection>(new SqlConnection(connection["RepairFirmaStorage"]));
builder.Services.AddSingleton(new RepairDbContext(connection["RepairFirma"]));
builder.Services.AddSingleton(new MetaDbContext(connection["RepairFirmaMeta"]));
builder.Services.AddSingleton(new StorageDbContext(connection["RepairFirmaStorage"]));
builder.Services.AddSingleton<IDbRepository, DbRepository>();


var loginData = builder.Configuration.GetSection("LoginAccount");
builder.Services.AddSingleton(loginData.Get<LoginData>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
