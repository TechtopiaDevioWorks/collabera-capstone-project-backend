namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Training;
using WebApi.Services;

[ApiController]
public class TrainingController : ControllerBase
{
    private ITrainingService _trainingService;
    private IMapper _mapper;

    public TrainingController(
        ITrainingService trainingService,
        IMapper mapper)
    {
        _trainingService = trainingService;
        _mapper = mapper;
    }

    [Route("training")]    
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        var trainings = _trainingService.GetAll(expand);
        return Ok(trainings);
    }
    [Route("training")]  
    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        _trainingService.Create(model);
        return Ok(new { message = "Training created" });
    }

    [Route("training/{id}")]
    [HttpGet()]
    public IActionResult GetById([FromRoute] int id)
    {
        var training = _trainingService.GetById(id);
        return Ok(training);
    }

    [Route("training/{id}")]  
    [HttpPut]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _trainingService.Update(id, model);
        return Ok(new { message = "Training updated" });
    }

    [Route("training/{id}")]  
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _trainingService.Delete(id);
        return Ok(new { message = "Training deleted" });
    }
}