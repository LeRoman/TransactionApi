using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TransactionApi.DatabaseContext;
using TransactionApi.Interfaces;
using TransactionApi.Services;

namespace TransactionApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TransactionAPI",
                    Description = "An ASP.NET Core Web API for managing transactions"
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            builder.Services.AddTransient<ICsvImportService, CsvImportService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddTransient<IExportService, ExportService>();

            builder.Services.AddDbContext<TransactionContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
