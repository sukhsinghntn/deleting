using Radzen;
using DynamicFormsApp.Server.Components;
using DynamicFormsApp.Client.Services;
using DynamicFormsApp.Shared.Services;
using DynamicFormsApp.Server.Services;
using DynamicFormsApp.Server.Data;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services for Blazor components, CORS, and DB contexts
builder.Services.AddRazorComponents()
      .AddInteractiveServerComponents()
      .AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024)
      .AddInteractiveWebAssemblyComponents();

// Enable response compression for efficient WASM payloads
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

// Register EF Core DbContext
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Dynamic form service
builder.Services.AddScoped<DynamicFormService>();

// CORS policy to allow any origin, method, and header
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<CookieHelper>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseCors("AllowAll");

// Configure Blazor components (server and WebAssembly rendering)
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode()
   .AddAdditionalAssemblies(typeof(DynamicFormsApp.Client._Imports).Assembly);

app.MapRazorPages();
app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();