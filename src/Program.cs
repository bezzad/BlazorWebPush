using BlazorWebPush.Components;
using BlazorWebPush.Models;
using BlazorWebPush.Services;

var builder = WebApplication.CreateBuilder(args);

// load configs from beginning
var vapid = builder.Configuration.GetSection("Vapid").Get<Vapid>();
builder.Services.AddSingleton(vapid);
builder.Services.AddSingleton<WebPushService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();