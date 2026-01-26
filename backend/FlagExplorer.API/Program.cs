using FlagExplorer.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<CountryService>();



// Habilita CORS para qualquer origem (pode restringir depois)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors(); // 🔹 importante: antes do MapControllers
app.UseAuthorization();
app.MapControllers();
// Retorna uma mensagem simples quando alguém acessar http://localhost:5144/
app.MapGet("/", () => "FlagExplorer API is running!");


app.Run("http://0.0.0.0:80");
