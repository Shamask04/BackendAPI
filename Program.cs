using PortfolioTrackerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register AlphaVantageService with interface
builder.Services.AddScoped<IAlphaVantageService, AlphaVantageService>();

// Register HTTP client
builder.Services.AddHttpClient<IAlphaVantageService, AlphaVantageService>();

builder.Services.AddSingleton<IEmailVerificationService, EmailVerificationService>();


// Enable CORS for Angular
builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS (for Angular at localhost:4200)
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();