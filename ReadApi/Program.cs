using PDBProject.Dal.Mongo.Configurations;
using PDBProject.Dal.Mongo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mongoDbSection = builder.Configuration.GetSection("MongoDB");
var databaseSettings = mongoDbSection.Get<DatabaseSettings>()!;

builder.Services.AddSingleton(databaseSettings);
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<ProductService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();