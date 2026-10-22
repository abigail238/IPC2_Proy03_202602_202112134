using IPC2_Proy03.Api.Servicios;
using QuestPDF.Infrastructure;


// Permite arrancar también desde bin sin guardar los datos en esa carpeta.
var carpetaProyecto = new DirectoryInfo(AppContext.BaseDirectory);
while (carpetaProyecto.Parent is not null &&
       !File.Exists(Path.Combine(carpetaProyecto.FullName, "IPC2_Proy03.Api.csproj")))
    carpetaProyecto = carpetaProyecto.Parent;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = File.Exists(Path.Combine(carpetaProyecto.FullName, "IPC2_Proy03.Api.csproj"))
        ? carpetaProyecto.FullName : Directory.GetCurrentDirectory()
});
builder.WebHost.UseUrls("http://localhost:5000");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<NitService>();
builder.Services.AddScoped<RtuService>();
builder.Services.AddScoped<AutorizacionesService>();
builder.Services.AddScoped<ConsultaService>();
builder.Services.AddScoped<ReporteService>();
builder.Services.AddScoped<SistemaService>();
builder.Services.AddScoped<PdfService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Las pruebas locales utilizan HTTP en el puerto 5000.

app.UseAuthorization();

app.MapControllers();

app.Run();

