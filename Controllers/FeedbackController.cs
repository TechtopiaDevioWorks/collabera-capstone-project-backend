namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Feedback;
using WebApi.Services;

[ApiController]
public class FeedbackController : ControllerBase
{
    private IFeedbackService _feedbackService;
    private IMapper _mapper;

    public FeedbackController(
        IFeedbackService feedbackService,
        IMapper mapper)
    {
        _feedbackService = feedbackService;
        _mapper = mapper;
    }

    [Route("feedback")]    
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        var feedbacks = _feedbackService.GetAll(expand);
        return Ok(feedbacks);
    }
    [Route("feedback")]  
    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        _feedbackService.Create(model);
        return Ok(new { message = "Feedback created" });
    }

    [Route("feedback/{id}")]
    [HttpGet()]
    public IActionResult GetById([FromRoute] int id)
    {
        var feedback = _feedbackService.GetById(id);
        return Ok(feedback);
    }
/*
    [Route("user/{id}")]  
    [HttpPut]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _userService.Update(id, model);
        return Ok(new { message = "User updated" });
    }
*/
    [Route("feedback/{id}")]  
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _feedbackService.Delete(id);
        return Ok(new { message = "Feedback deleted" });
    }
}