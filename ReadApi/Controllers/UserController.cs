using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PDBProject.Dal.Mongo.Entities;
using PDBProject.Dal.Mongo.Services;

namespace PDBProject.ReadApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:int}")]
    public async Task<Results<Ok<UserEntity>, NotFound>> Get(int id)
    {
        var existingUser = await _userService.GetAsync(id);
        if (existingUser is null) return TypedResults.NotFound();

        return TypedResults.Ok(existingUser);
    }

    [HttpGet]
    public async Task<Ok<List<UserEntity>>> Get()
    {
        var allUsers = await _userService.GetAsync();

        return TypedResults.Ok(allUsers);
    }
}