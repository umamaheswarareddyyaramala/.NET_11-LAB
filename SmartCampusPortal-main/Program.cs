using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CampusContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("CampusDb")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Creates the database and table automatically on first run
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<CampusContext>().Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ServiceRequests}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();