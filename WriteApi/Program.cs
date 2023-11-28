// create a builder that will create our needed application for us

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PDBProject.Common.Enums;
using PDBProject.Common.Models;
using PDBProject.WriteApi;

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
    if (dbContext.Database.GetPendingMigrations().Any()) dbContext.Database.Migrate();
}

var userGroup = app.MapGroup("user");
var productGroup = app.MapGroup("product");
var orderGroup = app.MapGroup("order");
var orderItemGroup = app.MapGroup("orderItem");
MapUsers(userGroup);
MapProducts(productGroup);
MapOrders(orderGroup);
MapOrderItems(orderItemGroup);

app.Run();

return;

void MapUsers(IEndpointRouteBuilder routeBuilder)
{
    routeBuilder.MapPost(string.Empty,
        Results<Ok<int>, BadRequest<string>> (Mapper mapper, EShopDbContext dbContext, UserModel userModel) =>
        {
            if (dbContext.Users.Any(user => user.Email == userModel.Email))
                return TypedResults.BadRequest("User with the same email already exists!");

            var insertedEntry = dbContext.Users.Add(mapper.ToEntity(userModel with { Id = 0 }));
            dbContext.SaveChanges();

            return TypedResults.Ok(insertedEntry.Entity.Id);
        });

    routeBuilder.MapPut(string.Empty,
        Results<Ok, NotFound<string>> (Mapper mapper, EShopDbContext dbContext, UserModel userModel) =>
        {
            if (dbContext.Users.All(user => user.Id != userModel.Id))
                return TypedResults.NotFound($"User with the id {userModel.Id} couldn't be found!");

            var userEntity = mapper.ToEntity(userModel);
            dbContext.Users.Update(userEntity);
            dbContext.SaveChanges();

            return TypedResults.Ok();
        });

    routeBuilder.MapDelete("{id:int}",
        Results<Ok, NotFound<string>> (EShopDbContext dbContext, int id) =>
        {
            if (dbContext.Users.Find(id) is not { } userToDelete)
                return TypedResults.NotFound($"User with the id {id} couldn't be found!");

            dbContext.Users.Remove(userToDelete);
            dbContext.SaveChanges();
            return TypedResults.Ok();
        });
}

void MapProducts(IEndpointRouteBuilder routeBuilder)
{
    routeBuilder.MapPost(string.Empty,
        Ok<int> (Mapper mapper, EShopDbContext dbContext, ProductModel productModel) =>
        {
            var insertedEntry = dbContext.Products.Add(mapper.ToEntity(productModel with { Id = 0 }));
            dbContext.SaveChanges();

            return TypedResults.Ok(insertedEntry.Entity.Id);
        });

    routeBuilder.MapPut(string.Empty,
        Results<Ok, NotFound<string>> (Mapper mapper, EShopDbContext dbContext,
            ProductModel productModel) =>
        {
            if (dbContext.Products.All(product => product.Id != productModel.Id))
                return TypedResults.NotFound($"Product with the id {productModel.Id} couldn't be found!");

            var productEntity = mapper.ToEntity(productModel);
            dbContext.Products.Update(productEntity);
            dbContext.SaveChanges();

            return TypedResults.Ok();
        });

    routeBuilder.MapDelete("{id:int}",
        Results<Ok, NotFound<string>> (EShopDbContext dbContext, int id) =>
        {
            if (dbContext.Products.Find(id) is not { } productToDelete)
                return TypedResults.NotFound($"Product with the id {id} couldn't be found!");

            dbContext.Products.Remove(productToDelete);
            dbContext.SaveChanges();
            return TypedResults.Ok();
        });
}

void MapOrders(IEndpointRouteBuilder routeBuilder)
{
    routeBuilder.MapPost(string.Empty,
        Ok<int> (Mapper mapper, EShopDbContext dbContext, OrderModel orderModel) =>
        {
            var insertedEntry = dbContext.Orders.Add(mapper.ToEntity(orderModel with { Id = 0 }));
            dbContext.SaveChanges();

            return TypedResults.Ok(insertedEntry.Entity.Id);
        });

    routeBuilder.MapPost("{id:int}/checkout",
        Results<Ok, BadRequest<string>, NotFound<string>> (EShopDbContext dbContext, int id) =>
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
            dbContext.SaveChanges();

            return TypedResults.Ok();
        });

    routeBuilder.MapPost("{id:int}/ship",
        Results<Ok, BadRequest<string>, NotFound<string>> (EShopDbContext dbContext, int id) =>
        {
            if (dbContext.Orders.Find(id) is not { } orderEntity)
                return TypedResults.NotFound($"Order with the id {id} couldn't be found!");

            if (orderEntity.State != OrderState.Ordered)
                return TypedResults.BadRequest($"Cannot ship order in state {orderEntity.State}!");

            orderEntity.State = OrderState.Shipped;
            orderEntity.ShippedDate = DateTime.UtcNow;
            dbContext.Orders.Update(orderEntity);
            dbContext.SaveChanges();

            return TypedResults.Ok();
        });

    routeBuilder.MapPost("{id:int}/confirm",
        Results<Ok, BadRequest<string>, NotFound<string>> (EShopDbContext dbContext, int id) =>
        {
            if (dbContext.Orders.Find(id) is not { } orderEntity)
                return TypedResults.NotFound($"Order with the id {id} couldn't be found!");

            if (orderEntity.State != OrderState.Shipped)
                return TypedResults.BadRequest($"Cannot confirm order in state {orderEntity.State}!");

            orderEntity.State = OrderState.Received;
            orderEntity.ReceivedDate = DateTime.UtcNow;
            dbContext.Orders.Update(orderEntity);
            dbContext.SaveChanges();

            return TypedResults.Ok();
        });

    routeBuilder.MapDelete("{id:int}",
        Results<Ok, NotFound<string>> (EShopDbContext dbContext, int id) =>
        {
            if (dbContext.Orders.Find(id) is not { } orderToDelete)
                return TypedResults.NotFound($"Order with the id {id} couldn't be found!");

            dbContext.Orders.Remove(orderToDelete);
            dbContext.SaveChanges();
            return TypedResults.Ok();
        });
}

void MapOrderItems(IEndpointRouteBuilder routeBuilder)
{
    routeBuilder.MapPost(string.Empty,
        Results<Ok, BadRequest<string>, NotFound<string>> (Mapper mapper, EShopDbContext dbContext,
            OrderItemModel orderItemModel) =>
        {
            if (dbContext.Orders.Find(orderItemModel.OrderId) is not { } orderEntity)
                return TypedResults.NotFound($"Order with the id {orderItemModel.OrderId} couldn't be found!");

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
                    var newEntity = mapper.ToEntity(orderItemModel);
                    dbContext.OrderItems.Add(newEntity);
                }
            }

            dbContext.SaveChanges();
            return TypedResults.Ok();
        });
}