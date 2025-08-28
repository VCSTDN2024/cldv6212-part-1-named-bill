var builder = WebApplication.CreateBuilder(args);

// Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetSection("AzureStorage:ConnectionString").Value;

// Add services to the container for Dependency Injection.
builder.Services.AddSingleton(new azuresolution1.Services.TableStorageService("customerprofiles", connectionString));
builder.Services.AddSingleton(new azuresolution1.Services.TableStorageService("productinfo", connectionString));
builder.Services.AddSingleton(new azuresolution1.Services.BlobStorageService("productimages", connectionString));
builder.Services.AddSingleton(new azuresolution1.Services.QueueStorageService("orderqueue", connectionString));
builder.Services.AddSingleton(new azuresolution1.Services.FileStorageService("contracts", connectionString));

builder.Services.AddControllersWithViews();

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();