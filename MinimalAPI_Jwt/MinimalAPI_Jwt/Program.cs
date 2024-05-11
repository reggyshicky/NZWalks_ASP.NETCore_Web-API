using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication("Cookie")
    .AddCookie("Cookie");

var app = builder.Build();
app.UseAuthentication();
app.MapGet("/username", (HttpContext ctx) =>
{
    if (ctx.User != null)
    {
        var userClaim = ctx.User.FindFirst("usr");
        if (userClaim != null)
        {
            return userClaim.Value;
        }
    }
    return "User or user claim not found";
});
app.MapGet("/login", async (HttpContext ctx) =>
{
    var claims = new List<Claim>();
    claims.Add(new Claim("usr", "reginah"));
    var identity = new ClaimsIdentity(claims, "Cookie");
    var user = new ClaimsPrincipal(identity);
    await ctx.SignInAsync("Cookie", user);
    return "ok";
});

app.Run();

































//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.DataProtection;
//using System.Runtime.Intrinsics.Arm;
//using System.Security.Claims;

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDataProtection();
//builder.Services.AddScoped<AuthService>();
//builder.Services.AddHttpContextAccessor();

//var app = builder.Build();
//app.Use((ctx, next) =>
//{
//    var idp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
//    var protector = idp.CreateProtector("auth-cookie");
//    var authCookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.StartsWith("auth="));
//    if (authCookie != null)
//    {
//        var protectedPayload = authCookie.Split("=").Last();
//        var payload = protector.Unprotect(protectedPayload);
//        var parts = payload.Split(":");
//        var key = parts[0];
//        var value = parts[1];

//        var claims = new List<Claim>();
//        claims.Add(new Claim(key, value));
//        var identity = new ClaimsIdentity(claims);
//        ctx.User = new ClaimsPrincipal(identity);
//        // Rest of your code
//    }
//    else
//    {
//        Console.WriteLine("Login to generate cookie");
//    }
//    return next();
//});


//app.MapGet("/username", (HttpContext ctx) =>
//{
//    return ctx.User.FindFirst("usr").Value;
//});


//app.MapGet("/login", (AuthService auth) =>
//{
//    auth.SignIn();
//    return "ok";
//});


//app.Run();

//public class AuthService
//{
//    private readonly IDataProtectionProvider _idp;
//    private readonly IHttpContextAccessor _accessor;
//    public AuthService(IDataProtectionProvider idp, IHttpContextAccessor accessor)
//    {
//        _idp = idp;
//        _accessor = accessor;
//    }

//    public void SignIn()
//    {
//        var protector = _idp.CreateProtector("auth-cookie");
//        _accessor.HttpContext.Response.Headers["set-cookie"] = $"auth={protector.Protect("usr:reginah")}";

//    }
//}
