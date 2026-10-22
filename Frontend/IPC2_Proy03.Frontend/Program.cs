using IPC2_Proy03.Frontend.Servicios;

var builder =
    WebApplication.CreateBuilder(args);


// Habilitamos Razor Pages.
builder.Services.AddRazorPages();


// Registramos ApiService junto con HttpClient.
//
// Cuando una Razor Page necesite ApiService,
// ASP.NET Core lo creará automáticamente.
builder.Services.AddHttpClient<ApiService>();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}


// El frontend local se comunica con la API mediante HTTP.

app.UseRouting();

app.UseAuthorization();


// Permite servir CSS, JS, imágenes, etc.
app.MapStaticAssets();


// Habilitamos las Razor Pages.
app.MapRazorPages()
   .WithStaticAssets();


app.Run();
