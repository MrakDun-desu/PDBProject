using Riok.Mapperly.Abstractions;
using WriteApi.Entities;
using WriteApi.Models;

namespace WriteApi;

[Mapper]
public partial class Mapper
{
    public partial UserEntity ToEntity(UserModel model);
    public partial UserModel ToModel(UserEntity entity);

    public partial ProductEntity ToEntity(ProductModel model);
    public partial ProductModel ToModel(ProductEntity entity);
    
    public partial OrderEntity ToEntity(OrderModel model);
    public partial OrderModel ToModel(OrderEntity entity);
    
    public partial OrderItemEntity ToEntity(OrderItemModel model);
    public partial OrderItemModel ToModel(OrderItemEntity entity);
}