using System;
using System.Threading;
using System.Threading.Tasks;
using Abstracciones.Interfaces.Flujo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class LimpiezaCarritos : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public LimpiezaCarritos(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            
                using var scope = _scopeFactory.CreateScope();
                var carritoFlujo = scope.ServiceProvider.GetRequiredService<ICarritoFlujo>();
                int minutosExpiracion = 10;
                await carritoFlujo.EliminarCarritosExpirados(minutosExpiracion);
                await Task.Delay(TimeSpan.FromMinutes(100), stoppingToken);
            
        }
    }
}
