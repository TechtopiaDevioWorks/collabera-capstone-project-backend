namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models.Training;
using System.Security.Claims;
using WebApi.Services;
using Microsoft.AspNetCore.OData.Query;

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
    [Authorize(AuthenticationSchemes = "CustomScheme")]
    [Route("training")]    
    [HttpGet]
    [EnableQuery()]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        string roleId = User.FindFirstValue("role_id");
        string teamId = User.FindFirstValue("team_id");
        var trainings = _trainingService.GetAll(roleId, teamId, expand);
        return Ok(trainings);
    }

    [Authorize(AuthenticationSchemes = "CustomScheme")]
    [Route("training/count")]    
    [HttpGet]
    public IActionResult GetAllCount()
    {
        var trainingCount = _trainingService.GetAllCount();
        return Ok(trainingCount);
    }

    [Authorize(AuthenticationSchemes = "CustomScheme")]
    [Route("training-filtered/count")]
    [HttpGet]
    public async Task<IActionResult> GetAllCountFiltered(ODataQueryOptions<WebApi.Entities.Training> odataQueryOptions)
    {
        string roleId = User.FindFirstValue("role_id");
        string teamId = User.FindFirstValue("team_id");

        var queryable = _trainingService.GetAll(roleId, teamId, false).AsQueryable();
        if(odataQueryOptions != null) {
        var filteredQueryable = odataQueryOptions.Filter.ApplyTo(queryable, new ODataQuerySettings()) as IQueryable<WebApi.Entities.Training>;
        var count = await Task.FromResult(filteredQueryable.Count());
        return Ok(count);
        }else{
            return Ok(queryable.Count());
        }
    }


    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [Route("training")]  
    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        _trainingService.Create(model);
        return Ok(new { message = "Training created" });
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHrOrManager")]
    [Route("training/{id}")]
    [HttpGet()]
    public IActionResult GetById([FromRoute] int id)
    {
        var training = _trainingService.GetById(id);
        return Ok(training);
    }

    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [Route("training/{id}")]  
    [HttpPut]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _trainingService.Update(id, model);
        return Ok(new { message = "Training updated" });
    }
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [Route("training/{id}")]  
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _trainingService.Delete(id);
        return Ok(new { message = "Training deleted" });
    }
}