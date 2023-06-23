using Application.IService;
using Application.Services;
using Domain.IRepositories;
using Infra.Context;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = String.Format("ApiPaymnet"),
        Description = "Api Payment to E-Commerce.",
        Version = "v1"
    });

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "ApiPayment.xml");
    c.IncludeXmlComments(filePath);
});

//DB
builder.Services.AddDbContext<DBPaymentContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Services
builder.Services.AddTransient<ISaleService, SaleService>();

//Repositories
builder.Services.AddTransient<ISaleRepository, SaleRepository>();
builder.Services.AddTransient<ISellerRepository, SellerRepository>();
builder.Services.AddTransient<IItemRepository, ItemRepository>();

//interfaces
builder.Services.AddTransient<IDBPaymentContext, DBPaymentContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Payment V1");
    c.RoutePrefix = "api-docs";
});

app.UseAuthorization();

app.MapControllers();

app.Run();
