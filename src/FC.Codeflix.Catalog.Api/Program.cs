using FC.Codeflix.Catalog.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddAndConfigConnections(builder.Configuration)
       .AddUseCases()
       .AddRabbitMQ(builder.Configuration)
       .AddMessageProducer()
       .AddMessageConsumer()
       .AddStorage(builder.Configuration)
       .AddSecurity(builder.Configuration)
       .AddAndConfigureControllers(builder.Configuration);
     //  .AddCors(p => p.AddPolicy("CORS", builder =>
     //  {
     //      builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
     //  }))
     //;

var app = builder.Build();
app.UseDocumentation();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
