using UserAuthBackend.Data;
using UserAuthBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<UserService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("https://clubpet.vercel.app") // Permitir apenas do front-end
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Permitir cookies ou autenticação baseada em sessão, se necessário
    });
});

var app = builder.Build();

app.UseCors("AllowLocalhost");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
