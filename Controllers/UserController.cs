namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.User;
using WebApi.Services;

[ApiController]
//[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserService _userService;
    private IMapper _mapper;

    public UserController(
        IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [Route("user")]    
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        var users = _userService.GetAll(expand);
        return Ok(users);
    }

    [Route("user/{id}")]
    [HttpGet()]
    public IActionResult GetById([FromRoute] int id, [FromQuery] bool expand = false)
    {
        var user = _userService.GetById(id, expand);
        return Ok(user);
    }

    [Route("login")]
    [HttpPut]
    public IActionResult Login(LoginRequest model) {
        var user = _userService.Login(model);
        return Ok(user);
    }

    [Route("user")]  
    [HttpPost]
    public IActionResult Create(RegisterRequest model)
    {
        _userService.Create(model);
        return Ok(new { message = "User created" });
    }

    [Route("user/{id}")]  
    [HttpPut]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _userService.Update(id, model);
        return Ok(new { message = "User updated" });
    }

    [Route("user/{id}")]  
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return Ok(new { message = "User deleted" });
    }
}