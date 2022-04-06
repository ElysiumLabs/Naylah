using Naylah.ConsoleAspNetCore;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Environment, builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app);

app.Run();
