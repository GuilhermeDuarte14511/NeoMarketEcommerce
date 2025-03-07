using NeoMarket.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adiciona os serviços de aplicação usando a extensão criada
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Rota personalizada para a URL amigável com urlSlug
app.MapControllerRoute(
    name: "storeRoute",
    pattern: "{urlSlug}",
    defaults: new { controller = "Home", action = "Index" }
);

// Rota padrão para demais controllers e ações
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
