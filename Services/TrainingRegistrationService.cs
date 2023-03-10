namespace WebApi.Services;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.TrainingRegistration;
using System;
using System.Text;
using AutoMapper;

public interface ITrainingRegistrationService
{
    IEnumerable<TrainingRegistration> GetAll(Boolean expand = false);
    void Create(CreateRequest model);
    TrainingRegistration GetById(int id, Boolean expand = false);
    void Delete(int id);
}

public class TrainingRegistrationService : ITrainingRegistrationService
{
    private DataContext _context;
    private readonly IMapper _mapper;
    private ISharedService _sharedService;

    public TrainingRegistrationService(
        DataContext context,
        IMapper mapper)
    {
        _sharedService = new SharedService(context, mapper);
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<TrainingRegistration> GetAll(Boolean expand = false)
    {
        if (expand == true)
            return _context.TrainingRegistration.Include(t => t.Status).Include(t => t.Training).Include(t => t.User);
        else
            return _context.TrainingRegistration;
    }

    public void Create(CreateRequest model)
    {
        if (!_context.User.Any(x => x.id == model.user_id))
            throw new AppException("User does not exist");
        if (!_context.Training.Any(x => x.id == model.training_id))
            throw new AppException("Training does not exist");
        if (_context.TrainingRegistration.Any(x => x.training_id == model.training_id && x.user_id == model.user_id)) {
            throw new AppException("User already registered to this training");
        }
        var trainingRegistration = _mapper.Map<TrainingRegistration>(model);
        trainingRegistration.status_id = 1;
        trainingRegistration.registration_date = DateTime.Now;
        Console.WriteLine($"{trainingRegistration.user_id}");
        _context.TrainingRegistration.Add(trainingRegistration);
        _context.SaveChanges();
    }

    public TrainingRegistration GetById(int id, bool expand = false)
    {
        return _sharedService.GetTrainingRegistration(id);
    }

    public void Delete(int id)
    {
        var trainingRegistration = _sharedService.GetTrainingRegistration(id);
        _context.TrainingRegistration.Remove(trainingRegistration);
        _context.SaveChanges();
    }

}