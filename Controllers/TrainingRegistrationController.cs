namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models.TrainingRegistration;
using WebApi.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.OData.Query;

[ApiController]
public class TrainingRegistrationController : ControllerBase
{
    private ITrainingRegistrationService _trainingRegistrationService;
    private IMapper _mapper;

    public TrainingRegistrationController(
        ITrainingRegistrationService trainingRegistrationService,
        IMapper mapper)
    {
        _trainingRegistrationService = trainingRegistrationService;
        _mapper = mapper;
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHrOrManager")]
    [Route("training-registration")]    
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        string roleId = User.FindFirstValue("role_id");
        if (roleId == "3")
        {
            var trainingRegistrations = _trainingRegistrationService.GetAll(expand);
            return Ok(trainingRegistrations);
        }
        else if (roleId == "2")
        {
            string teamId = User.FindFirstValue("team_id");
            var trainingRegistrations = _trainingRegistrationService.GetAllTeam(teamId, expand);
            return Ok(trainingRegistrations);
        }
        else return NotFound();
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHrOrManager")]
    [Route("training-registration/training/{id}")]    
    [HttpGet]
    public IActionResult GetAllByTraining([FromRoute] int id, [FromQuery] bool expand = false)
    {
        string roleId = User.FindFirstValue("role_id");
        if (roleId == "3")
        {
            var trainingRegistrations = _trainingRegistrationService.GetAllByTraining(id, expand);
            return Ok(trainingRegistrations);
        }
        else if (roleId == "2")
        {
            string teamId = User.FindFirstValue("team_id");
            var trainingRegistrations = _trainingRegistrationService.GetAllTeamByTraining(id, teamId, expand);
            return Ok(trainingRegistrations);
        }
        else return NotFound();
    }

    [Authorize(AuthenticationSchemes = "CustomScheme")]
    [Route("training-registration-status")]    
    [HttpGet]
    public IActionResult GetAllStatus()
    {
        var trainingRegistrationStatus = _trainingRegistrationService.GetStatusList();
        return Ok(trainingRegistrationStatus);
    }


    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isEmployee")]
    [Route("training-registration")]  
    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        _trainingRegistrationService.Create(model);
        return Ok(new { message = "Training registration created" });
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [Route("training-registration/{id}")]
    [HttpGet()]
    public IActionResult GetById([FromRoute] int id)
    {
        var trainingRegistration = _trainingRegistrationService.GetById(id);
        return Ok(trainingRegistration);
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHrOrManager")]
    [Route("training-registration/{id}")]  
    [HttpPut]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _trainingRegistrationService.Update(id, model);
        return Ok(new { message = "Training registration updated" });
    }

    [Authorize(AuthenticationSchemes = "CustomScheme")]
    [Route("training-registration/{id}")]  
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        string roleId = User.FindFirstValue("role_id");
        string teamId = User.FindFirstValue("team_id");
        int userId = -1;
        try {
        userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        catch {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Could not identify user." });
        }
        _trainingRegistrationService.Delete(userId, roleId, teamId, id);
        return Ok(new { message = "Training registration deleted" });
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHrOrManager")]
    [Route("training-history/{id}")]
    [HttpGet()]
    public IActionResult GetUserTrainingHistory([FromRoute] int id)
    {
        var trainingHistory = _trainingRegistrationService.GetUserTrainingHistory(id);
        return Ok(trainingHistory);
    }

    [Authorize(AuthenticationSchemes = "CustomScheme")]
    [Route("training-history")]
    [HttpGet()]
    public IActionResult GetUserTrainingHistoryPersonal()
    {
        int userId = -1;
        try {
        userId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        catch {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Could not identify user." });
        }
        var trainingHistory = _trainingRegistrationService.GetUserTrainingHistory(userId);
        return Ok(trainingHistory);
    }
}