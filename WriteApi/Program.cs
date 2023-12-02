using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PDBProject.Dal.Common.Enums;
using PDBProject.Dal.Common.Models;
using PDBProject.Dal.Mongo.Configurations;
using PDBProject.Dal.Mongo.Services;
using PDBProject.WriteApi;

// create a builder that will create our needed application for us
var builder = WebApplication.CreateBuilder(args);

// get the connection string from configuration and connect to database
// if we're running for development, we'll get it through the appSettings.json,
// if we're running for production (in docker), we'll get it through environment variable
// (builder can automatically get the correctly named env variable)
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EShopDbContext>(options => options.UseNpgsql(connStr));

builder.Services.AddSingleton<Mapper>();
var mongoDbSection = builder.Configuration.GetSection("MongoDB");
var databaseSettings = mongoDbSection.Get<DatabaseSettings>()!;

builder.Services.AddSingleton(databaseSettings);
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<ProductService>();

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
    if (dbContext.Database.GetPendingMigrations().Any()) dbContext.Database.Migrate();
}

var userGroup = app.MapGroup("User");
var productGroup = app.MapGroup("Product");
var orderGroup = app.MapGroup("Order");
var orderItemGroup = app.MapGroup("OrderItem");
MapUsers(userGroup);
MapProducts(productGroup);
MapOrders(orderGroup);
MapOrderItems(orderItemGroup);

app.Run();

return;

void MapUsers(IEndpointRouteBuilder routeBuilder)
{
    routeBuilder.MapPost(string.Empty,
        async Task<Results<Ok<int>, ProblemHttpResult>>
            (Mapper mapper, EShopDbContext dbContext, UserModel userModel, UserService userService) =>
        {
            var insertedEntry = dbContext.Users.Add(mapper.ToPostgresEntity(userModel with { Id = 0 }));
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            var insertedEntity = insertedEntry.Entity;

            var mongoEntity = mapper.ToMongoEntity(insertedEntity);

            await userService.CreateAsync(mongoEntity);

            return TypedResults.Ok(insertedEntry.Entity.Id);
        });

    routeBuilder.MapPut(string.Empty,
        async Task<Results<Ok, NotFound<string>, ProblemHttpResult>>
            (Mapper mapper, EShopDbContext dbContext, UserService userService, UserModel userModel) =>
        {
            if (dbContext.Users.All(user => user.Id != userModel.Id))
                return TypedResults.NotFound($"User with the id {userModel.Id} couldn't be found!");

            var userEntity = mapper.ToPostgresEntity(userModel);
            dbContext.Users.Update(userEntity);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            var mongoEntity = mapper.ToMongoEntity(userEntity);

            await userService.UpdateAsync(mongoEntity);

            return TypedResults.Ok();
        });

    routeBuilder.MapDelete("{id:int}",
        async Task<Results<Ok, NotFound<string>, ProblemHttpResult>>
            (EShopDbContext dbContext, UserService userService, int id) =>
        {
            if (dbContext.Users.Find(id) is not { } userToDelete)
                return TypedResults.NotFound($"User with the id {id} couldn't be found!");

            dbContext.Users.Remove(userToDelete);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            await userService.RemoveAsync(id);

            return TypedResults.Ok();
        });
}

void MapProducts(IEndpointRouteBuilder routeBuilder)
{
    routeBuilder.MapPost(string.Empty,
        async Task<Results<Ok<int>, ProblemHttpResult>>
            (Mapper mapper, EShopDbContext dbContext, ProductService productService, ProductModel productModel) =>
        {
            var lowercaseCategories = productModel.Categories.Select(cat => cat.ToLowerInvariant()).ToArray();
            var insertedEntry =
                dbContext.Products.Add(mapper.ToPostgresEntity(productModel with
                {
                    Id = 0, Categories = lowercaseCategories
                }));
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            var insertedEntity = insertedEntry.Entity;
            var mongoEntity = mapper.ToMongoEntity(insertedEntity);
            await productService.CreateAsync(mongoEntity);

            return TypedResults.Ok(insertedEntry.Entity.Id);
        });

    routeBuilder.MapPut(string.Empty,
        async Task<Results<Ok, NotFound<string>, ProblemHttpResult>>
            (Mapper mapper, EShopDbContext dbContext, ProductService productService, ProductModel productModel) =>
        {
            if (dbContext.Products.All(product => product.Id != productModel.Id))
                return TypedResults.NotFound($"Product with the id {productModel.Id} couldn't be found!");

            var lowercaseCategories = productModel.Categories.Select(cat => cat.ToLowerInvariant()).ToArray();
            var productEntity = mapper.ToPostgresEntity(productModel with { Categories = lowercaseCategories });
            dbContext.Products.Update(productEntity);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            var mongoEntity = mapper.ToMongoEntity(productEntity);
            await productService.UpdateAsync(mongoEntity);

            return TypedResults.Ok();
        });

    routeBuilder.MapDelete("{id:int}",
        async Task<Results<Ok, NotFound<string>, ProblemHttpResult>>
            (EShopDbContext dbContext, ProductService productService, int id) =>
        {
            if (dbContext.Products.Find(id) is not { } productToDelete)
                return TypedResults.NotFound($"Product with the id {id} couldn't be found!");

            dbContext.Products.Remove(productToDelete);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            await productService.RemoveAsync(id);

            return TypedResults.Ok();
        });
}

void MapOrders(IEndpointRouteBuilder routeBuilder)
{
    routeBuilder.MapPost(string.Empty,
        async Task<Results<Ok<int>, ProblemHttpResult>>
            (Mapper mapper, EShopDbContext dbContext, UserService userService, OrderModel orderModel) =>
        {
            var insertedEntry = dbContext.Orders.Add(mapper.ToPostgresEntity(orderModel with { Id = 0 }));
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            var updatedUser = dbContext.Users
                .Include(user => user.Orders)
                .ThenInclude(order => order.OrderItems)
                .Single(user => user.Id == orderModel.UserId);

            var mongoEntity = mapper.ToMongoEntity(updatedUser);

            await userService.UpdateAsync(mongoEntity);

            return TypedResults.Ok(insertedEntry.Entity.Id);
        });

    routeBuilder.MapPost("{id:int}/checkout",
        async Task<Results<Ok, BadRequest<string>, NotFound<string>, ProblemHttpResult>>
            (EShopDbContext dbContext, Mapper mapper, UserService userService, int id) =>
        {
            var orderEntity = dbContext.Orders
                .Include(order => order.OrderItems)
                .ThenInclude(orderItemEntity => orderItemEntity.Product)
                .SingleOrDefault(order => order.Id == id);
            if (orderEntity is null) return TypedResults.NotFound($"Order with the id {id} couldn't be found!");

            if (orderEntity.State != OrderState.InBasket)
                return TypedResults.BadRequest($"Cannot checkout order in state {orderEntity.State}!");

            foreach (var orderItem in orderEntity.OrderItems)
            {
                if (orderItem.ProductCount > orderItem.Product.StockCount)
                    return TypedResults.BadRequest($"Not enough of product {orderItem.Product.Name} in stock!");

                orderItem.Product.StockCount -= orderItem.ProductCount;
                dbContext.Products.Update(orderItem.Product);
            }

            orderEntity.State = OrderState.Ordered;
            orderEntity.OrderedDate = DateTime.UtcNow;
            dbContext.Orders.Update(orderEntity);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            var updatedUser = dbContext.Users
                .Include(user => user.Orders)
                .ThenInclude(order => order.OrderItems)
                .Single(user => user.Id == orderEntity.UserId);

            var mongoEntity = mapper.ToMongoEntity(updatedUser);

            await userService.UpdateAsync(mongoEntity);

            return TypedResults.Ok();
        });

    routeBuilder.MapPost("{id:int}/ship",
        async Task<Results<Ok, BadRequest<string>, NotFound<string>, ProblemHttpResult>>
            (EShopDbContext dbContext, UserService userService, Mapper mapper, int id) =>
        {
            if (dbContext.Orders.Find(id) is not { } orderEntity)
                return TypedResults.NotFound($"Order with the id {id} couldn't be found!");

            if (orderEntity.State != OrderState.Ordered)
                return TypedResults.BadRequest($"Cannot ship order in state {orderEntity.State}!");

            orderEntity.State = OrderState.Shipped;
            orderEntity.ShippedDate = DateTime.UtcNow;
            dbContext.Orders.Update(orderEntity);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            var updatedUser = dbContext.Users
                .Include(user => user.Orders)
                .ThenInclude(order => order.OrderItems)
                .Single(user => user.Id == orderEntity.UserId);

            var mongoEntity = mapper.ToMongoEntity(updatedUser);

            await userService.UpdateAsync(mongoEntity);

            return TypedResults.Ok();
        });

    routeBuilder.MapPost("{id:int}/confirm",
        async Task<Results<Ok, BadRequest<string>, NotFound<string>, ProblemHttpResult>>
            (EShopDbContext dbContext, Mapper mapper, UserService userService, int id) =>
        {
            if (dbContext.Orders.Find(id) is not { } orderEntity)
                return TypedResults.NotFound($"Order with the id {id} couldn't be found!");

            if (orderEntity.State != OrderState.Shipped)
                return TypedResults.BadRequest($"Cannot confirm order in state {orderEntity.State}!");

            orderEntity.State = OrderState.Received;
            orderEntity.ReceivedDate = DateTime.UtcNow;
            dbContext.Orders.Update(orderEntity);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            var updatedUser = dbContext.Users
                .Include(user => user.Orders)
                .ThenInclude(order => order.OrderItems)
                .Single(user => user.Id == orderEntity.UserId);

            var mongoEntity = mapper.ToMongoEntity(updatedUser);

            await userService.UpdateAsync(mongoEntity);

            return TypedResults.Ok();
        });

    routeBuilder.MapDelete("{id:int}",
        async Task<Results<Ok, NotFound<string>, ProblemHttpResult>>
            (EShopDbContext dbContext, UserService userService, Mapper mapper, int id) =>
        {
            if (dbContext.Orders.Find(id) is not { } orderToDelete)
                return TypedResults.NotFound($"Order with the id {id} couldn't be found!");

            var changedUserId = orderToDelete.UserId;

            dbContext.Orders.Remove(orderToDelete);
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            var updatedUser = dbContext.Users
                .Include(user => user.Orders)
                .ThenInclude(order => order.OrderItems)
                .Single(user => user.Id == changedUserId);

            var mongoEntity = mapper.ToMongoEntity(updatedUser);

            await userService.UpdateAsync(mongoEntity);

            return TypedResults.Ok();
        });
}

void MapOrderItems(IEndpointRouteBuilder routeBuilder)
{
    routeBuilder.MapPost(string.Empty,
        async Task<Results<Ok, BadRequest<string>, NotFound<string>, ProblemHttpResult>>
            (Mapper mapper, EShopDbContext dbContext, UserService userService, OrderItemModel orderItemModel) =>
        {
            if (dbContext.Orders.Find(orderItemModel.OrderId) is not { } orderEntity)
                return TypedResults.NotFound($"Order with the id {orderItemModel.OrderId} couldn't be found!");

            if (dbContext.Products.Find(orderItemModel.ProductId) is null)
                return TypedResults.NotFound($"Product with the id {orderItemModel.ProductId} couldn't be found!");

            if (orderEntity.State != OrderState.InBasket)
                return TypedResults.BadRequest("Cannot change products, order no longer in basket!");

            if (dbContext.OrderItems.Find(orderItemModel.ProductId, orderItemModel.OrderId) is { } orderItemEntity)
            {
                if (orderItemModel.ProductCount == 0)
                    dbContext.OrderItems.Remove(orderItemEntity);
                else
                    orderItemEntity.ProductCount = orderItemModel.ProductCount;
            }
            else
            {
                if (orderItemModel.ProductCount != 0)
                {
                    var newEntity = mapper.ToPostgresEntity(orderItemModel);
                    dbContext.OrderItems.Add(newEntity);
                }
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return TypedResults.Problem("Couldn't save changes to the database!");
            }

            var updatedUser = dbContext.Users
                .Include(user => user.Orders)
                .ThenInclude(order => order.OrderItems)
                .Single(user => user.Id == orderEntity.UserId);

            var mongoEntity = mapper.ToMongoEntity(updatedUser);

            await userService.UpdateAsync(mongoEntity);

            return TypedResults.Ok();
        });
}