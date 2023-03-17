namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models.Attendance;
using WebApi.Services;
using System.Security.Claims;

[ApiController]
public class AttendanceController : ControllerBase
{
    private IAttendanceService _attendanceService;
    private IMapper _mapper;

    public AttendanceController(
        IAttendanceService attendanceService,
        IMapper mapper)
    {
        _attendanceService = attendanceService;
        _mapper = mapper;
    }
    [Authorize(AuthenticationSchemes = "CustomScheme")]
    [Route("attendance")]
    [HttpGet]
    public IActionResult GetAll([FromQuery] int user_id, [FromQuery] int training_id)
    {
        int currentUserId = -1;
        string roleId = User.FindFirstValue("role_id");
        string teamId = User.FindFirstValue("team_id");
        try {
        currentUserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        catch {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Could not identify user." });
        }
        var attendances = _attendanceService.GetAll(currentUserId, roleId, teamId, user_id, training_id);
        return Ok(attendances);
    }
    [Authorize(AuthenticationSchemes = "CustomScheme")]
    [Route("attendance")]
    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        int userId = -1;
        try {
        userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        catch {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Could not identify user." });
        }
        model.user_id = userId;
        _attendanceService.Create(model);
        return Ok(new { message = "Attendance created" });
    }
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [Route("attendance/{id}")]
    [HttpGet()]
    public IActionResult GetById([FromRoute] int id)
    {
        var attendance = _attendanceService.GetById(id);
        return Ok(attendance);
    }
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [Route("attendance/{id}")]
    [HttpPut]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _attendanceService.Update(id, model);
        return Ok(new { message = "Attendance updated" });
    }
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [Route("attendance/{id}")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _attendanceService.Delete(id);
        return Ok(new { message = "Attendance deleted" });
    }
}