using Microsoft.EntityFrameworkCore;
using PupuseriaSalvadorena.Conexion;
using PupuseriaSalvadorena.Repositorios.Interfaces;
using PupuseriaSalvadorena.Repositorios.Implementaciones;
using Hangfire;
using PupuseriaSalvadorena.Services;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MiDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Repositorios
builder.Services.AddScoped<ICorreosRep, CorreosRep>();
builder.Services.AddScoped<IProvinciasRep, ProvinciasRep>();
builder.Services.AddScoped<ICantonesRep, CantonesRep>();
builder.Services.AddScoped<IDistritosRep, DistritosRep>();
builder.Services.AddScoped<IDireccionesRep, DireccionesRep>();
builder.Services.AddScoped<ITelefonosRep, TelefonosRep>();
builder.Services.AddScoped<IPersonasRep, PersonasRep>();
builder.Services.AddScoped<IRolRep, RolRep>();
builder.Services.AddScoped<IUsuariosRep, UsuariosRep>();
builder.Services.AddScoped<INegociosRep, NegociosRep>();
builder.Services.AddScoped<IRegistrosBancariosRep, RegistrosBancariosRep>();
builder.Services.AddScoped<IImpuestosRep, ImpuestosRep>();
builder.Services.AddScoped<ITipoTransacRep, TipoTransacRep>();
builder.Services.AddScoped<IRegistroLibrosRep, RegistroLibrosRep>();
builder.Services.AddScoped<IDetallesTransacRep, DetallesTransacRep>();
builder.Services.AddScoped<IConciliacionRep, ConciliacionRep>();
builder.Services.AddScoped<ICatPresupuestoRep, CatPresupuestoRep>();
builder.Services.AddScoped<IPresupuestoRep, PresupuestoRep>();
builder.Services.AddScoped<IDetallesPresupuestoRep, DetallesPresupuestoRep>();
builder.Services.AddScoped<IDeclaracionTaxRep, DeclaracionTaxRep>();
builder.Services.AddScoped<ITipoPagoRep, TipoPagoRep>();
builder.Services.AddScoped<ITipoFacturaRep, TipoFacturaRep>();
builder.Services.AddScoped<IFacturaVentaRep, FacturaVentaRep>();
builder.Services.AddScoped<IFacturaCompraRep, FacturaCompraRep>();
builder.Services.AddScoped<IEnvioFacturaRep, EnvioFacturaRep>();
builder.Services.AddScoped<IProveedorRep, ProveedorRep>();
builder.Services.AddScoped<IMateriaPrimaRep, MateriaPrimaRep>();
builder.Services.AddScoped<IHistorialCompraRep, HistorialCompraRep>();
builder.Services.AddScoped<IHistorialVentaRep, HistorialVentaRep>();
builder.Services.AddScoped<ICuentaPagarRep, CuentaPagarRep>();
builder.Services.AddScoped<IDetallesCuentaRep, DetallesCuentaRep>();
builder.Services.AddScoped<IPlatilloRep, PlatilloRep>();
builder.Services.AddScoped<ITipoVentaRep, TipoVentaRep>();
builder.Services.AddScoped<IPronosticoRep, PronosticoRep>();
builder.Services.AddScoped<ITipoMovimientoRep, TipoMovimientoRep>();
builder.Services.AddScoped<IAlertaCuentaPagarRep, AlertaCuentaPagarRep>();
builder.Services.AddScoped<IDetallesPronosticoRep, DetallesPronosticoRep>();

//Servicios
builder.Services.AddScoped<ServicioPronosticos>();
builder.Services.AddHostedService<AlertaCuentaPagarServ>();

//Hangfire
builder.Services.AddHangfire(config => config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

RotativaConfiguration.Setup(app.Environment.ContentRootPath, "Rotativa/Windows");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseHangfireDashboard();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=IniciarSesion}/{id?}");

app.Run();
