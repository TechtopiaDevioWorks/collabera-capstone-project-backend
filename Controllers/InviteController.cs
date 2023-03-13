namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Invite;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class InviteController : ControllerBase
{
    private IInviteService _inviteService;
    private IMapper _mapper;

    public InviteController(
        IInviteService inviteService,
        IMapper mapper)
    {
        _inviteService = inviteService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var invites = _inviteService.GetAll();
        return Ok(invites);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var invite = _inviteService.GetById(id);
        return Ok(invite);
    }

    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        _inviteService.Create(model);
        return Ok(new { message = "Invite created" });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _inviteService.Update(id, model);
        return Ok(new { message = "Invite updated" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _inviteService.Delete(id);
        return Ok(new { message = "Invite deleted" });
    }
}