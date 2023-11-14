using ElectronNET.API;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseElectron(args);
builder.Services.AddElectron();

if (HybridSupport.IsElectronActive)
{
    // Open the Electron-Window here
    _ = Task.Run(async () => {
        var window = await Electron.WindowManager.CreateWindowAsync();

        window.WebContents.OpenDevTools();

        window.OnClosed += () => {
            Electron.App.Quit();
        };
    });
}

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

app.MapRazorComponents<SimpleBlazorWebApp.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();