namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models.Feedback;
using WebApi.Services;
using System.Security.Claims;

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
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [Route("feedback")]
    [HttpGet]
    public IActionResult GetAll([FromQuery] bool expand = false)
    {
        var feedbacks = _feedbackService.GetAll(expand);
        return Ok(feedbacks);
    }
    [Authorize(AuthenticationSchemes = "CustomScheme")]
    [Route("feedback")]
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
        model.from_user_id = userId;
        _feedbackService.Create(model);
        return Ok(new { message = "Feedback created" });
    }
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
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
    [Authorize(AuthenticationSchemes = "CustomScheme", Policy = "isHr")]
    [Route("feedback/{id}")]
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _feedbackService.Delete(id);
        return Ok(new { message = "Feedback deleted" });
    }
}