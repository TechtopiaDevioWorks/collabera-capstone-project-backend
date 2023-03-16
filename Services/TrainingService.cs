namespace WebApi.Services;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Training;
using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.OData.Query;

public interface ITrainingService
{
    IEnumerable<Training> GetAll(string roleId, string teamId, Boolean expand = false);
    void Create(CreateRequest model);
    int GetAllCount();
    Training GetById(int id);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}

public class TrainingService : ITrainingService
{
    private DataContext _context;
    private readonly IMapper _mapper;
    private ISharedService _sharedService;

    public TrainingService(
        DataContext context,
        IMapper mapper)
    {
        _sharedService = new SharedService(context, mapper);
        _context = context;
        _mapper = mapper;
    }
    [EnableQuery()]
    public IEnumerable<Training> GetAll(string roleId, string teamId, Boolean expand = false)
    {
        if (roleId == "1")
        {
            return _context.Training.Include(t => t.Status);
        }
        else if (roleId == "2")
        {
            if (teamId == null) throw new KeyNotFoundException("Invalid teamid.");
            Team team = null;
            try
            {
                team = _sharedService.GetTeam(Byte.Parse(teamId));
            }
            catch
            {
                throw new KeyNotFoundException("Invalid teamid.");
            }
            return _context.Training.Include(t => t.Status).Include(t => t.TrainingRegistrations).ThenInclude(t => t.User).ThenInclude(u => u.Team).Select(u => new TrainingViewAdmin
            {
                id = u.id,
                description = u.description,
                start = u.start,
                end = u.end,
                name = u.name,
                Status = u.Status,
                min_hours = u.min_hours,
                NoTrainingRegistrations = u.TrainingRegistrations.Where(tr => tr.User.Team == team).Count()
            });
        }
        else if (roleId == "3")
        {
            return _context.Training.Include(t => t.Status).Include(t => t.TrainingRegistrations).Select(u => new TrainingViewAdmin
            {
                id = u.id,
                description = u.description,
                start = u.start,
                end = u.end,
                name = u.name,
                Status = u.Status,
                min_hours = u.min_hours,
                NoTrainingRegistrations = u.TrainingRegistrations.Count()
            });
        } else {
             throw new AppException("Invalid role/team");
        }
    }
    public int GetAllCount()
    {
        return _context.Training.Count();
    }
    public void Create(CreateRequest model)
    {
        var training = _mapper.Map<Training>(model);
        training.status_id = 1;
        _context.Training.Add(training);
        _context.SaveChanges();
    }

    public Training GetById(int id)
    {
        return _sharedService.GetTraining(id);
    }

    public void Update(int id, UpdateRequest model)
    {
        var training = _sharedService.GetTraining(id);
        if (model.status_id != null)
            _sharedService.GetTrainingStatus(model.status_id);
        else model.status_id = training.status_id;
        if (model.start != null && model.end != null)
        {
            if (model.start > model.end)
                throw new AppException("End should be after start");
        }
        else
        {
            model.start = training.start;
            model.end = training.end;
        }
        if (model.min_hours == null) model.min_hours = training.min_hours;
        _mapper.Map(model, training);
        _context.Training.Update(training);
        _context.SaveChanges();
    }


    public void Delete(int id)
    {
        var training = _sharedService.GetTraining(id);
        var userscount = _context.TrainingRegistration.Where(t => t.training_id == id).Count();
        if (userscount > 0) throw new AppException("A training with applicants can't be deleted.");
        _context.Training.Remove(training);
        _context.SaveChanges();
    }

}