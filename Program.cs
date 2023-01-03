var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
var app = builder.Build();

app.UseStaticFiles();
app.UseMvcWithDefaultRoute();

app.UseMvc(routes =>
{
    routes.MapRoute(
    name: "default",
    template: "{controller=FileManager}/{action=Index}");
});

app.Run();