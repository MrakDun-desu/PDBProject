
// create a builder that will create our needed application for us

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WriteApi;
using WriteApi.Entities;
using WriteApi.Models;

var builder = WebApplication.CreateBuilder(args);

// get the connection string from configuration and connect to database
// if we're running for development, we'll get it through the appsettings.json,
// if we're running for production (in docker), we'll get it through environment variable
// (builder can automatically get the correctly named env variable)
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EShopDbContext>(options => options.UseNpgsql(connStr));
// note: for mongodb, you *probably* won't use entity framework since that's mostly used for relational dbs.
// instead of adding DB context to services, you'll just add whatever service you'll need to provide 
// interaction with mongodb

// add swagger to the builder (web UI for making the requests)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

// check if database is up to date with our current entities, if not, migrate it (update the tables)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EShopDbContext>();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}

// if you need to use something in your handler, you can simply add it as a parameter and as long
// as it's been added to services before, the application will automatically give you what you need.
// here you can see that I'm injecting dbcontext that gives me database operations
app.MapGet("user", (EShopDbContext dbContext) => {
    return dbContext.Users.ToList();
    // you can just return whatever you want, it will be serialized automatically
});
// if you want parametric mapping, you can just add them like this
app.MapGet("user/{id:int}", (EShopDbContext dbContext, int id) => {
    if (dbContext.Users.Find(id) is { } user)
    {
        return Results.Ok(user);
    }

    return Results.NotFound($"User with id {id} couldn't be found!");
});
// if you want to accept some POST body in JSON format, just add it to lambda signature
app.MapPost("user", (EShopDbContext dbContext, UserModel userModel) =>
{
    // mapping the model to entity because we might want to accept different data types in http than have in table
    // this can be automatized but I'm lazy so I'm just leaving it like this for now
    var userEntity = new UserEntity
    {
        Id = userModel.Id,
        Address = userModel.Address,
        Email = userModel.Email,
        Name = userModel.Name
    };

    var addedEntry = dbContext.Users.Add(userEntity);
    dbContext.SaveChanges();

    return addedEntry.Entity.Id;
});
// if you want to accept some POST body in JSON format, just add it to lambda signature
// here you can also see that you can enumerate the possible results of the request (these will show up in web UI)
app.MapDelete("user/{id:int}", Results<Ok, NotFound<string>> (EShopDbContext dbContext, int id) =>
{
    var foundUser = dbContext.Users.Find(id);
    if (foundUser is null) return TypedResults.NotFound($"User with id {id} couldn't be found!");
    
    dbContext.Remove(foundUser);
    dbContext.SaveChanges();
    return TypedResults.Ok();

});

app.MapGet("order", (EShopDbContext dbContext) => dbContext.Orders.ToList());
app.MapGet("orderItem", (EShopDbContext dbContext) => dbContext.OrderItems.ToList());
app.MapGet("product", (EShopDbContext dbContext) => dbContext.Products.ToList());
app.MapGet("category", (EShopDbContext dbContext) => dbContext.Categories.ToList());


app.Run();
