using PDBProject.Dal.Common.Models;
using PDBProject.WriteApi.Entities;
using Riok.Mapperly.Abstractions;
using MongoEntities = PDBProject.Dal.Mongo.Entities;

namespace PDBProject.WriteApi;

[Mapper]
public partial class Mapper
{
    public partial UserEntity ToPostgresEntity(UserModel model);
    public partial MongoEntities.UserEntity ToMongoEntity(UserEntity postgresEntity);
    public partial UserModel ToModel(UserEntity entity);

    public partial ProductEntity ToPostgresEntity(ProductModel model);
    public partial MongoEntities.ProductEntity ToMongoEntity(ProductEntity postgresEntity);
    public partial ProductModel ToModel(ProductEntity entity);

    public partial OrderEntity ToPostgresEntity(OrderModel model);
    public partial MongoEntities.OrderEntity ToMongoEntity(OrderEntity postgresEntity);
    public partial OrderModel ToModel(OrderEntity entity);

    public partial OrderItemEntity ToPostgresEntity(OrderItemModel model);
    public partial MongoEntities.OrderItemEntity ToMongoEntity(OrderItemEntity postgresEntity);
    public partial OrderItemModel ToModel(OrderItemEntity entity);
}