using FC.Codeflix.Catalog.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddAndConfigConnections(builder.Configuration)
       .AddUseCases()
       .AddRabbitMQ(builder.Configuration)
       .AddMessageProducer()
       .AddMessageConsumer()
       .AddAndConfigureControllers()
       .AddStorage(builder.Configuration)
     ;

var app = builder.Build();
app.UseDocumentation();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
