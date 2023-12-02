using PDBProject.Dal.Common.Models;
using PDBProject.WriteApi.Entities;
using Riok.Mapperly.Abstractions;
using MongoEntities = PDBProject.Dal.Mongo.Entities;

namespace PDBProject.WriteApi;

/// <summary>
/// Automatic mapper for all the mapping needed in the WriteAPI.
/// All the method implementations are source-generated at runtime.
/// </summary>
[Mapper]
public partial class Mapper
{
    public partial UserEntity ToPostgresEntity(UserModel model);
    public partial MongoEntities.UserEntity ToMongoEntity(UserEntity postgresEntity);

    public partial ProductEntity ToPostgresEntity(ProductModel model);
    public partial MongoEntities.ProductEntity ToMongoEntity(ProductEntity postgresEntity);

    public partial OrderEntity ToPostgresEntity(OrderModel model);

    public partial OrderItemEntity ToPostgresEntity(OrderItemModel model);
}