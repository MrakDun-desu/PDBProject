// create a builder that will create our needed application for us

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WriteApi;
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

builder.Services.AddSingleton<Mapper>();

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

var userGroup = app.MapGroup("user");
MapUsers(userGroup);

app.Run();

return;

void MapUsers(IEndpointRouteBuilder routeBuilder)
{
    routeBuilder.MapPost(string.Empty,
        Results<Ok<int>, BadRequest<string>> (Mapper mapper, EShopDbContext dbContext, UserModel userModel) =>
        {
            if (dbContext.Users.Any(user => user.Email == userModel.Email))
            {
                return TypedResults.BadRequest("User with the same email already exists!");
            }

            var insertedEntry = dbContext.Users.Add(mapper.ToEntity(userModel with { Id = 0 }));
            dbContext.SaveChanges();

            return TypedResults.Ok(insertedEntry.Entity.Id);
        });

    routeBuilder.MapPut(string.Empty,
        Results<Ok<UserModel>, NotFound<string>> (Mapper mapper, EShopDbContext dbContext, UserModel userModel) =>
        {
            if (dbContext.Users.All(user => user.Id != userModel.Id))
            {
                return TypedResults.NotFound($"User with the id {userModel.Id} couldn't be found!");
            }

            var userEntity = mapper.ToEntity(userModel);
            var updatedEntry = dbContext.Users.Update(userEntity);
            dbContext.SaveChanges();

            return TypedResults.Ok(mapper.ToModel(updatedEntry.Entity));
        });

    routeBuilder.MapDelete("{id:int}",
        Results<Ok, NotFound<string>> (EShopDbContext dbContext, int id) =>
        {
            if (dbContext.Users.Find(id) is not { } userToDelete)
            {
                return TypedResults.NotFound($"User with the id {id} couldn't be found!");
            }

            dbContext.Users.Remove(userToDelete);
            dbContext.SaveChanges();
            return TypedResults.Ok();
        });
}