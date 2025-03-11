using NeoMarket.Infrastructure.DependencyInjection;
using NeoMarket.Filters;
using NeoMarket.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configurar a sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registrar o filtro global e os serviços
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<StoreDataFilter>(); // Adiciona o filtro global para carregar dados da loja
});

// Adiciona os serviços de aplicação usando a extensão criada
builder.Services.AddApplicationServices(builder.Configuration);

// Adiciona o HttpClient para o serviço Melhor Envio
builder.Services.AddMelhorEnvioHttpClient(builder.Configuration);

// Registrar o IHttpContextAccessor e o IStoreService para o filtro
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configurar o pipeline de requisição
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); // Ativa o uso de sessão

// Adicionando o middleware do carrinho
app.UseMiddleware<CartMiddleware>();

app.UseRouting();
app.UseAuthorization();

// Rota para produtos por subcategoria
app.MapControllerRoute(
    name: "subcategoria",
    pattern: "produtos/{subCategorySlug}",
    defaults: new { controller = "Product", action = "Index" }
);

// Rota personalizada para lojas
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
