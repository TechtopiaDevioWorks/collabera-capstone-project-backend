namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public IActionResult GetAll()
    {
        var teams = _teamService.GetAll();
        return Ok(teams);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var team = _teamService.GetById(id);
        return Ok(team);
    }

    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        _teamService.Create(model);
        return Ok(new { message = "Team created" });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _teamService.Update(id, model);
        return Ok(new { message = "Team updated" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _teamService.Delete(id);
        return Ok(new { message = "Team deleted" });
    }
}