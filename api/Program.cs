// This line creates a WebApplicationBuilder object, which is 
// responsible for configuring the web application. It sets up 
// the application's services, configuration, logging, and other
// settings.
using api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// This adds the controller services to the service container, 
// which allows the application to handle incoming HTTP requests
// using controller-based architecture (MVC-like pattern).
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(opt => 
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// This middleware forces all HTTP requests to be redirected to HTTPS.
app.UseHttpsRedirection();

// Adds authorization middleware to the request pipeline.
// It ensures that the endpoints of the API can only be 
// accessed by authorized users (if authorization policies
// are configured).
app.UseAuthorization();

// Maps the controller endpoints to the request pipeline, 
// so the application can route incoming HTTP requests to 
// the appropriate controller actions.
app.MapControllers();


var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

try
{
    context.Database.Migrate();
    DbInitializer.Initialize(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during migration");
}

// Starts the application and begins listening for incoming 
// HTTP requests.
app.Run();
