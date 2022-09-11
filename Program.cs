using Microsoft.EntityFrameworkCore;
using src.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Banco de dados
builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseInMemoryDatabase("dbContracts"));

// Toda vez que chamar um DatabaseContext, envia uma nova instância de DatabaseContext
// Injeção de dependência. Lá no construtor do controller é configurado 
builder.Services.AddScoped<DatabaseContext, DatabaseContext>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
