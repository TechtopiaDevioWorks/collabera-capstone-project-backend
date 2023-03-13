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
    IEnumerable<TrainingRegistrationView> GetAll(Boolean expand = false);
    void Create(CreateRequest model);
    TrainingRegistration GetById(int id, Boolean expand = false);
    void Update(int id, UpdateRequest model);
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

    public IEnumerable<TrainingRegistrationView> GetAll(Boolean expand = false)
    {
        if (expand == true)
            return _context.TrainingRegistration.Include(t => t.Status).Include(t => t.Training).Include(t => t.User).Select(tr => new TrainingRegistrationViewMax {
                id=tr.id,
                user_id=tr.user_id,
                training_id=tr.training_id,
                registration_date=tr.registration_date,
                status_id=tr.status_id,
                Training=tr.Training,
                Status=tr.Status,
                User = new UserView{
                    id = tr.User.id,
                    username = tr.User.username,
                    firstname = tr.User.firstname,
                    lastname = tr.User.lastname,
                    email = tr.User.email,
                }
            });
        else
            return _context.TrainingRegistration.Select(tr => new TrainingRegistrationView{
                id=tr.id,
                user_id=tr.user_id,
                training_id=tr.training_id,
                registration_date=tr.registration_date,
                status_id=tr.status_id
            });
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
        _context.TrainingRegistration.Add(trainingRegistration);
        _context.SaveChanges();
    }

    public TrainingRegistration GetById(int id, bool expand = false)
    {
        return _sharedService.GetTrainingRegistration(id);
    }

    public void Update(int id, UpdateRequest model)
    {
        var trainingRegistration = _sharedService.GetTrainingRegistration(id);
        _sharedService.GetTrainingRegistrationStatus(model.status_id);

        _mapper.Map(model, trainingRegistration);
        _context.TrainingRegistration.Update(trainingRegistration);
        _context.SaveChanges();
    }


    public void Delete(int id)
    {
        var trainingRegistration = _sharedService.GetTrainingRegistration(id);
        _context.TrainingRegistration.Remove(trainingRegistration);
        _context.SaveChanges();
    }

}