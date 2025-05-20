using ClienteAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
//Walter Salas -VII CICLO
var builder = WebApplication.CreateBuilder(args);
var dbHealthGauge = Metrics.CreateGauge("healthcheck_sqlserver", "Estado del HealthCheck de SQL Server (1 = Healthy, 0 = Unhealthy)");

// Configuración de la cadena de conexión y DbContext
builder.Services.AddDbContext<BdClientesContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ClienteDB")));

// Agregar controladores
builder.Services.AddControllers();

// Agregar HealthChecks para la base de datos SQL Server
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: builder.Configuration.GetConnectionString("ClienteDB") 
                          ?? throw new InvalidOperationException("Missing connection string: ClienteDB"),
        name: "sqlserver",
        tags: new[] { "db", "sql", "sqlserver" }
    );

// Swagger y endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware Prometheus
app.UseMetricServer();
app.UseHttpMetrics();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

// Endpoint para HealthChecks (JSON)
app.MapGet("/health", async context =>
{
    var healthCheckService = context.RequestServices.GetRequiredService<HealthCheckService>();
    var report = await healthCheckService.CheckHealthAsync();

    context.Response.ContentType = "text/plain";

    foreach (var entry in report.Entries)
    {
        var metricName = $"healthcheck_{entry.Key.ToLower()}";
        var status = entry.Value.Status == HealthStatus.Healthy ? 1 : 0;

        await context.Response.WriteAsync($"# HELP {metricName} Estado del HealthCheck de {entry.Key} (1 = Healthy, 0 = Unhealthy)\n");
        await context.Response.WriteAsync($"# TYPE {metricName} gauge\n");
        await context.Response.WriteAsync($"{metricName} {status}\n");
    }
});

// Mapear controladores
app.MapControllers();

app.Run();