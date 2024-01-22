using System.Text;
using eKoncert.Data;
using eKoncert.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));





builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRazorPages();

//Databases
builder.Services.AddDbContext<UserDbContext>();
builder.Services.AddDbContext<EventDbContext>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var eventContext = serviceProvider.GetRequiredService<EventDbContext>();

    eventContext.Database.EnsureCreated();

    if (!eventContext.Events.Any())
    {
        eventContext.Events.Add(new Event
        {
            Id = 1,
            Name = "Sample Event 1",
            Url = "/images/concerts/concert1",
        });
        eventContext.Events.Add(new Event
        {
            Id = 2,
            Name = "Sample Event 2",
            Url = "/images/concerts/concert2",
        });
        eventContext.Events.Add(new Event
        {
            Id = 3,
            Name = "Sample Event 3",
            Url = "/images/concerts/concert3",
        });

        eventContext.SaveChanges();
    }
}



app.Run(); // Keep app.Run() at the end
