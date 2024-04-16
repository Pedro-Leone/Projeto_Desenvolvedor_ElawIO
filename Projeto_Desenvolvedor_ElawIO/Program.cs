using Microsoft.EntityFrameworkCore;
using Projeto_Desenvolvedor_ElawIO.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Conexão Bannco
builder.Services.AddDbContext<Banco_Context>
    (options => options.UseSqlServer("Data Source=LAPTOP-89OBKOUS;Initial Catalog=Projeto_Desenvolvedor_ElawIO;Integrated Security=True; TrustServerCertificate=True"));

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dados}/{action=Index}/{id?}");

app.Run();
