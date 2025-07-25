using FC.Codeflix.Catalog.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddUseCases()
       .AddAndConfigureControllers()
       .AddAndConfigConnections(builder.Configuration);

var app = builder.Build();
app.UseDocumentation();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
                
app.Run();

public partial class Program { }
