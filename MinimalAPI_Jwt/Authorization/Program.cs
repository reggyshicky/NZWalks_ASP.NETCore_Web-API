
const string AuthScheme = "cookie";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(AuthScheme)
    .AddCookie(AuthScheme)
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
