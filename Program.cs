using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using GamePortal;
using GamePortal.Services;
using GamePortal.Hubs;
using GamePortal.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<GameService>();
builder.Services.AddHostedService<UserPresenceService>();
builder.Services.AddHostedService<GameCleanerService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<GameHub>("/gamehub");
app.MapFallbackToPage("/_Host");

app.Run();
