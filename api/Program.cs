using api.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Explicitly configure URLs here to ensure it's listening on the correct port
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // Ensure port is set correctly; this should match your desired port in launch.json or launchSettings.json
    serverOptions.ListenLocalhost(5000); // Use port 5000 for HTTP
});

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR services.
builder.Services.AddSignalR();

// Add CORS policy to allow your frontend to connect
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Update with your frontend URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

// Enable Swagger for API documentation
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS for frontend communication
app.UseCors("AllowReactApp");

// Map controllers
app.MapControllers();

// Map SignalR Hub
app.MapHub<QuizHub>("/quizHub");


app.Run();