using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<MovieRentalDBContext>
    (
    options => options.UseSqlServer
        (
            builder.Configuration.GetConnectionString("MovieRentalContext") ?? throw new InvalidOperationException("Connection string 'MovieRentalContext' not found.")
        )
    );
builder.Services.AddTransient<EmailSender>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddSingleton(new TokenService("MovieRental"));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Member/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();	

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Member}/{action=login}");

app.Run();
