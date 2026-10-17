using Application.Interfaces;
using Infra;
using Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.UseCases.Clientes;

namespace IOC;

 public static class BootStrapper
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<AuthService>();

            // var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            //     ?? configuration["ConnectionStrings:DefaultConnection"]
            //     ?? configuration.GetConnectionString("DefaultConnection");

            // if (string.IsNullOrWhiteSpace(connectionString))
            // {
            //     throw new InvalidOperationException("A connection string 'DefaultConnection' must be configured.");
            // }
            
            // // Console.WriteLine($"Using connection string: {connectionString}");
             string connStr = "Server=localhost;Port=3307;Database=ClienteVeiculo;User=appuser;Password=clienteveiculoservice;";
           // string connStr = "Server=mysql-clienteveiculo;Port=3307;Database=ClienteVeiculo;User=appuser;Password=clienteveiculoservice;";
Console.WriteLine($"Using connection string: {connStr}");
            services.AddDbContext<EFContext>(options =>
                options.UseMySQL(connStr));
            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EFContext>();
            dbContext.Database.Migrate();


            services.AddTransient<ClienteRepository, ClienteRepositoryImp>();
            services.AddTransient<VeiculoRepository, VeiculoRepositoryImp>();
            services.AddScoped<AdicionarClienteHandler>();
            services.AddScoped<AdicionarVeiculoClienteHandler>();            
            services.AddScoped<AtualizarClienteHandler>();
            services.AddScoped<AtualizarVeiculoClienteHandler>();
            services.AddScoped<BuscarVeiculoPlacaClienteHandler>();
            services.AddScoped<InativarClienteHandler>();
            services.AddScoped<InativarVeiculoClienteHandler>();
            services.AddScoped<VerificaCadastroClienteHandler>();
            services.AddScoped<BuscarVeiculoIdClienteHandler>();
        }
    }
