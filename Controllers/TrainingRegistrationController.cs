namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models.TrainingRegistration;
using WebApi.Services;
using System.Security.Claims;

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

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [Route("training-registration")]    
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        var trainingRegistrations = _trainingRegistrationService.GetAll(expand);
        return Ok(trainingRegistrations);
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
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

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [Route("training-registration/{id}")]  
    [HttpPut]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _trainingRegistrationService.Update(id, model);
        return Ok(new { message = "Training registration updated" });
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHR")]
    [Route("training-registration/{id}")]  
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _trainingRegistrationService.Delete(id);
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