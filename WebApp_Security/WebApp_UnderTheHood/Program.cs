using Microsoft.AspNetCore.Authorization;
using WebApp_UnderTheHood.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication("ReggyCookieAuth").AddCookie("ReggyCookieAuth", options =>
{
    options.Cookie.Name = "ReggyCookieAuth";
    options.ExpireTimeSpan = TimeSpan.FromSeconds(200);
    //options.LoginPath = "/Account1/Login";
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HRManagerOnly", policy => policy.RequireClaim("Department", "HR")
    .RequireClaim("Manager")
    .Requirements.Add(new HRManagerProbationRequirement(3)));
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
    options.AddPolicy("MustBelongToHrDepartment", policy => policy.RequireClaim("Department", "HR"));
    
});

builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
