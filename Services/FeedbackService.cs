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
        var feedback = _mapper.Map<Feedback>(model);
        _context.Feedback.Add(feedback);
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