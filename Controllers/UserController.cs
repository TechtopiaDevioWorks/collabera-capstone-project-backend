namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApi.Models.User;
using WebApi.Services;
using System.Text.Json;

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
    [Authorize(Policy = "isHrOrManager")]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        string roleId = User.FindFirstValue("role_id");
        if (roleId == "3")
        {
            var users = _userService.GetAll(expand);
            return Ok(users);
        }
        else if (roleId == "2")
        {
            string teamId = User.FindFirstValue("team_id");
            var users = _userService.GetAllTeam(teamId, expand);
            return Ok(users);
        }
        else return NotFound();
    }


    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [Route("user/{id}")]
    [HttpGet()]
    public IActionResult GetById([FromRoute] int id, [FromQuery] bool expand = false)
    {
        var user = _userService.GetById(id, expand);
        return Ok(user);
    }

    [AllowAnonymous]
    [Route("login")]
    [HttpPut]
    public IActionResult Login(LoginRequest model)
    {
        var user = _userService.Login(model);
        return Ok(user);
    }

    [AllowAnonymous]
    [Route("token-login")]
    [HttpPut]
    public IActionResult Login([FromBody] JsonElement body)
    {
        string token = body.GetProperty("token").ToString();
        var user = _userService.LoginByToken(token);
        return Ok(user);
    }

    [AllowAnonymous]
    [Route("user")]
    [HttpPost]
    public IActionResult Create(RegisterRequest model)
    {
        _userService.Create(model);
        return Ok(new { message = "User created" });
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [Route("user/{id}")]
    [HttpPut]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _userService.Update(id, model);
        return Ok(new { message = "User updated" });
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [Route("user/{id}")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return Ok(new { message = "User deleted" });
    }
}