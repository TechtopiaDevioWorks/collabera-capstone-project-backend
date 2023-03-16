namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models.Attendance;
using WebApi.Services;

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
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [Route("attendance")]
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        var attendances = _attendanceService.GetAll(expand);
        return Ok(attendances);
    }
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [Route("attendance")]
    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
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