using BCU.Contoso.Timetable.ServiceDefaults;
using BCU.Contoso.Timetable.Web.Apis;
using BCU.Contoso.Timetable.Web.Components;
using MudBlazor.Services;
using Refit;

namespace BCU.Contoso.Timetable.Web;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add service defaults & Aspire client integrations.
        builder.AddServiceDefaults();

        builder.Services.AddMudServices();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddOutputCache();

        builder.Services.AddRefitClient<ITimetableApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https+http://apiservice"));

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.UseOutputCache();

        app.MapStaticAssets();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.MapDefaultEndpoints();

        app.Run();
    }
}