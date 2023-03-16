namespace WebApi.Services;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Feedback;
using System;
using System.Text;
using AutoMapper;

public interface IFeedbackService
{
    IEnumerable<Feedback> GetAll(Boolean expand = false);
    void Create(CreateRequest model);
    Feedback GetById(int id);
    void Delete(int id);
}

public class FeedbackService : IFeedbackService
{
    private DataContext _context;
    private readonly IMapper _mapper;
    private ISharedService _sharedService;

    public FeedbackService(
        DataContext context,
        IMapper mapper)
    {
        _sharedService = new SharedService(context, mapper);
        _context = context;
        _mapper = mapper;
    }
    public IEnumerable<Feedback> GetAll(Boolean expand = false)
    {
        if (expand == true)
            return _context.Feedback;
        else
            return _context.Feedback;
    }

    public void Create(CreateRequest model)
    {
        var feedbacktype = _sharedService.GetFeedbackType(model.type_id);
        Feedback feedback = null;
        switch(model.type_id) {
            case 1:
                feedback = _context.Feedback.Where(f=> f.type_id == model.type_id && f.from_user_id == model.from_user_id && f.to_training_id == model.to_training_id).FirstOrDefault();
                break;
            case 2:
                feedback = _context.Feedback.Where(f=> f.type_id == model.type_id && f.from_user_id == model.from_user_id && f.to_attendance_id == model.to_attendance_id).FirstOrDefault();
                break;
            case 3:
                feedback = _context.Feedback.Where(f=> f.type_id == model.type_id && f.from_user_id == model.from_user_id && f.to_user_id == model.to_user_id).FirstOrDefault();
                break;
            case 4:
                feedback = _context.Feedback.Where(f=> f.type_id == model.type_id && f.from_user_id == model.from_user_id && f.to_training_registration_id == model.to_training_registration_id).FirstOrDefault();
                break;
        }
        if(feedback == null) {
            feedback = _mapper.Map<Feedback>(model);
            _context.Feedback.Add(feedback);
        } else {
            _mapper.Map(model, feedback);
            _context.Feedback.Update(feedback);
        }
        _context.SaveChanges();
    }

    public Feedback GetById(int id)
    {
        return _sharedService.GetFeedback(id);
    }

    public void Delete(int id)
    {
        var feedback = _sharedService.GetFeedback(id);
        _context.Feedback.Remove(feedback);
        _context.SaveChanges();
    }

}