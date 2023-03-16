namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var invites = _inviteService.GetAll();
        return Ok(invites);
    }
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var invite = _inviteService.GetById(id);
        return Ok(invite);
    }
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        var invite = _inviteService.Create(model);
        return Ok(new { message = "Invite created", invite = invite });
    }
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _inviteService.Update(id, model);
        return Ok(new { message = "Invite updated" });
    }
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _inviteService.Delete(id);
        return Ok(new { message = "Invite deleted" });
    }
}