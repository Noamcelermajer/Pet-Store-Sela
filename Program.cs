using Microsoft.EntityFrameworkCore;
using pet_store_noamcelermajer.Models;  // Make sure this line is here
// ... other using statements

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddDbContext<PetShopContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PetShopConnection")));
// ... other 
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PetShopContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
