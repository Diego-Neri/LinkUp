using ControleDeContatos.Data;
using MySql.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ControleDeContatos.Repositorio;



var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MariaDB");


builder.Services.AddDbContext<BancoContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddScoped<IContatoRepositorio, ContatoRepositorio  >();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio  >();

// Add services to the container.
builder.Services.AddControllersWithViews();



var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
