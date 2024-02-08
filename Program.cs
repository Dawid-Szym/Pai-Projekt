using System.Text;
using eKoncert.Data;
using eKoncert.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using eKoncert.Pages.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRazorPages();

//Databases
builder.Services.AddDbContext<UserDbContext>();
builder.Services.AddDbContext<EventDbContext>();
builder.Services.AddDbContext<TicketsDbContext>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.Cookie.Name = "token";
        options.LogoutPath = "/Logout";
		options.AccessDeniedPath = "/Login";


	}).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.Zero

		};
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["token"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 52428800; // 50 MB max request size
});


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


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



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
