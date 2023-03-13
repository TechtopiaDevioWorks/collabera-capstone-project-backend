namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.TrainingRegistration;
using WebApi.Services;

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

    [Route("training-registration")]    
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        var trainingRegistrations = _trainingRegistrationService.GetAll(expand);
        return Ok(trainingRegistrations);
    }
    [Route("training-registration")]  
    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        _trainingRegistrationService.Create(model);
        return Ok(new { message = "Training registration created" });
    }

    [Route("training-registration/{id}")]
    [HttpGet()]
    public IActionResult GetById([FromRoute] int id)
    {
        var trainingRegistration = _trainingRegistrationService.GetById(id);
        return Ok(trainingRegistration);
    }

    [Route("training-registration/{id}")]  
    [HttpPut]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _trainingRegistrationService.Update(id, model);
        return Ok(new { message = "Training registration updated" });
    }

    [Route("training-registration/{id}")]  
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _trainingRegistrationService.Delete(id);
        return Ok(new { message = "Training registration deleted" });
    }
}