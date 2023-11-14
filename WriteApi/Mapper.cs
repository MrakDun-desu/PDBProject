using Riok.Mapperly.Abstractions;
using WriteApi.Entities;
using WriteApi.Models;

namespace WriteApi;

[Mapper]
public partial class Mapper
{
    public partial UserEntity ToEntity(UserRegisterModel model);
    public partial UserEntity ToEntity(UserModel model);

    public partial UserModel ToModel(UserEntity entity);
}