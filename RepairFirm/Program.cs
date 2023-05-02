using EfCoreRepository;
using RepairFirm.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddControllers();
builder.Services.AddSingleton(new RepairDbContext());

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
