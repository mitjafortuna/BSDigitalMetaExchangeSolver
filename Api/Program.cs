using DomainService;
using DomainService.Interface;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetaExchange API", Version = "v1" });
});
builder.Services.AddSingleton<IExchangeExecutionPlanService, ExchangeExecutionPlanService>();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetaExchange API v1"));
//}

app.UseAuthorization();
app.MapControllers();
app.Run();
