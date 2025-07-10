using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using DynamicFormsApp.Shared.Models;
using DynamicFormsApp.Client;
using DynamicFormsApp.Client.Services;
using DynamicFormsApp.Shared.Services;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });

builder.Services.AddScoped<IUserService, UserServiceProxy>();
builder.Services.AddScoped<IEmailService, EmailServiceProxy>();

builder.Services.AddScoped<CookieHelper>();

var host = builder.Build();await host.RunAsync();

