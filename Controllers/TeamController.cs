namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models.Team;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class TeamController : ControllerBase
{
    private ITeamService _teamService;
    private IMapper _mapper;

    public TeamController(
        ITeamService teamService,
        IMapper mapper)
    {
        _teamService = teamService;
        _mapper = mapper;
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var teams = _teamService.GetAll();
        return Ok(teams);
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [HttpGet("{id}")]
    public IActionResult GetById(byte id)
    {
        var team = _teamService.GetById(id);
        return Ok(team);
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        _teamService.Create(model);
        return Ok(new { message = "Team created" });
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [HttpPut("{id}")]
    public IActionResult Update(byte id, UpdateRequest model)
    {
        _teamService.Update(id, model);
        return Ok(new { message = "Team updated" });
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [HttpDelete("{id}")]
    public IActionResult Delete(byte id)
    {
        _teamService.Delete(id);
        return Ok(new { message = "Team deleted" });
    }
}