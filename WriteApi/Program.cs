
// create a builder that will create our needed application for us
var builder = WebApplication.CreateBuilder(args);

// add swagger to the builder (web UI for making the requests)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
