namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

    [Route("attendance")]    
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        var attendances = _attendanceService.GetAll(expand);
        return Ok(attendances);
    }
    [Route("attendance")]  
    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        _attendanceService.Create(model);
        return Ok(new { message = "Attendance created" });
    }

    [Route("attendance/{id}")]
    [HttpGet()]
    public IActionResult GetById([FromRoute] int id)
    {
        var attendance = _attendanceService.GetById(id);
        return Ok(attendance);
    }
/*
    [Route("login")]
    [HttpPut]
    public IActionResult Login(LoginRequest model) {
        var user = _userService.Login(model);
        return Ok(user);
    }


    [Route("user/{id}")]  
    [HttpPut]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _userService.Update(id, model);
        return Ok(new { message = "User updated" });
    }*/

    [Route("attendance/{id}")]  
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _attendanceService.Delete(id);
        return Ok(new { message = "Attendance deleted" });
    }
}