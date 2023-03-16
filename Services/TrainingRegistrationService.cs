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
    public IEnumerable<TrainingRegistrationView> GetAllTeam(string teamId, Boolean expand = false);
    IEnumerable<TrainingRegistrationView> GetAllByTraining(int trainingId, Boolean expand = false);
    public IEnumerable<TrainingRegistrationView> GetAllTeamByTraining(int trainingId, string teamId, Boolean expand = false);
    public IEnumerable<TrainingRegistrationStatus> GetStatusList();
    void Create(CreateRequest model);
    TrainingRegistration GetById(int id, Boolean expand = false);
    IEnumerable<TrainingRegistrationViewMax> GetUserTrainingHistory(int userid);
    void Update(int id, UpdateRequest model);
    void Delete(int userId, string roleId, string teamId, int id);
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

    public IEnumerable<TrainingRegistrationStatus> GetStatusList()
    {

        return _context.TrainingRegistrationStatus;
    }

    public IEnumerable<TrainingRegistrationView> GetAll(Boolean expand = false)
    {
        if (expand == true)
            return _context.TrainingRegistration.Include(t => t.Status).Include(t => t.Training).Include(t => t.User).Select(tr => new TrainingRegistrationViewMax
            {
                id = tr.id,
                user_id = tr.user_id,
                training_id = tr.training_id,
                registration_date = tr.registration_date,
                status_id = tr.status_id,
                Training = tr.Training,
                Status = tr.Status,
                User = new UserView
                {
                    id = tr.User.id,
                    username = tr.User.username,
                    firstname = tr.User.firstname,
                    lastname = tr.User.lastname,
                    email = tr.User.email,
                }
            });
        else
            return _context.TrainingRegistration.Select(tr => new TrainingRegistrationView
            {
                id = tr.id,
                user_id = tr.user_id,
                training_id = tr.training_id,
                registration_date = tr.registration_date,
                status_id = tr.status_id
            });
    }

    public IEnumerable<TrainingRegistrationView> GetAllTeam(string teamId, Boolean expand = false)
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
        if (expand == true)
            return _context.TrainingRegistration.Include(t => t.Status).Include(t => t.Training).Include(t => t.User).Where(u => u.User.team_id == team.id).Select(tr => new TrainingRegistrationViewMax
            {
                id = tr.id,
                user_id = tr.user_id,
                training_id = tr.training_id,
                registration_date = tr.registration_date,
                status_id = tr.status_id,
                Training = tr.Training,
                Status = tr.Status,
                User = new UserView
                {
                    id = tr.User.id,
                    username = tr.User.username,
                    firstname = tr.User.firstname,
                    lastname = tr.User.lastname,
                    email = tr.User.email,
                }
            });
        else
            return _context.TrainingRegistration.Include(t => t.User).Where(u => u.User.team_id == team.id).Select(tr => new TrainingRegistrationView
            {
                id = tr.id,
                user_id = tr.user_id,
                training_id = tr.training_id,
                registration_date = tr.registration_date,
                status_id = tr.status_id
            });
    }

    public IEnumerable<TrainingRegistrationView> GetAllByTraining(int trainingId, Boolean expand = false)
    {
        if (expand == true)
            return _context.TrainingRegistration.Where(u => u.training_id == trainingId).Include(t => t.Status).Include(t => t.Training).Include(t => t.User).Include(t => t.User.Team).Include(t => t.User.Role).Select(tr => new TrainingRegistrationViewMaxExpand
            {
                id = tr.id,
                user_id = tr.user_id,
                training_id = tr.training_id,
                registration_date = tr.registration_date,
                status_id = tr.status_id,
                Training = tr.Training,
                Status = tr.Status,
                User = new UserViewExpand
                {
                    id = tr.User.id,
                    username = tr.User.username,
                    firstname = tr.User.firstname,
                    lastname = tr.User.lastname,
                    email = tr.User.email,
                    Role = tr.User.Role,
                    Team = tr.User.Team
                }
            });
        else
            return _context.TrainingRegistration.Where(u => u.training_id == trainingId).Select(tr => new TrainingRegistrationView
            {
                id = tr.id,
                user_id = tr.user_id,
                training_id = tr.training_id,
                registration_date = tr.registration_date,
                status_id = tr.status_id
            });
    }

    public IEnumerable<TrainingRegistrationView> GetAllTeamByTraining(int trainingId, string teamId, Boolean expand = false)
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
        if (expand == true)
            return _context.TrainingRegistration.Where(u => u.training_id == trainingId).Include(t => t.Status).Include(t => t.Training).Include(t => t.User).Include(t => t.User.Team).Include(t => t.User.Role).Where(u => u.User.team_id == team.id).Select(tr => new TrainingRegistrationViewMaxExpand
            {
                id = tr.id,
                user_id = tr.user_id,
                training_id = tr.training_id,
                registration_date = tr.registration_date,
                status_id = tr.status_id,
                Training = tr.Training,
                Status = tr.Status,
                User = new UserViewExpand
                {
                    id = tr.User.id,
                    username = tr.User.username,
                    firstname = tr.User.firstname,
                    lastname = tr.User.lastname,
                    email = tr.User.email,
                    Role = tr.User.Role,
                    Team = tr.User.Team
                }
            });
        else
            return _context.TrainingRegistration.Where(u => u.training_id == trainingId).Include(t => t.User).Where(u => u.User.team_id == team.id).Select(tr => new TrainingRegistrationView
            {
                id = tr.id,
                user_id = tr.user_id,
                training_id = tr.training_id,
                registration_date = tr.registration_date,
                status_id = tr.status_id
            });
    }

    public void Create(CreateRequest model)
    {
        if (!_context.User.Any(x => x.id == model.user_id))
            throw new AppException("User does not exist");
        if (!_context.Training.Any(x => x.id == model.training_id))
            throw new AppException("Training does not exist");
        if (_context.TrainingRegistration.Any(x => x.training_id == model.training_id && x.user_id == model.user_id))
        {
            throw new AppException("User already registered to this training");
        }
        var trainingRegistration = _mapper.Map<TrainingRegistration>(model);
        trainingRegistration.status_id = 1;
        trainingRegistration.registration_date = DateTime.Now;
        _context.TrainingRegistration.Add(trainingRegistration);
        _context.SaveChanges();
    }

    public IEnumerable<TrainingRegistrationViewMax> GetUserTrainingHistory(int userid)
    {
        return _context.TrainingRegistration.Include(t => t.Status).Include(t => t.Training).Include(t => t.User).Where(t => t.user_id == userid).Select(tr => new TrainingRegistrationViewMax
        {
            id = tr.id,
            user_id = tr.user_id,
            training_id = tr.training_id,
            registration_date = tr.registration_date,
            status_id = tr.status_id,
            Training = tr.Training,
            Status = tr.Status,
            User = new UserView
            {
                id = tr.User.id,
                username = tr.User.username,
                firstname = tr.User.firstname,
                lastname = tr.User.lastname,
                email = tr.User.email,
            }
        });
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


    public void Delete(int userId, string roleId, string teamId, int id)
    {
        TrainingRegistration trainingRegistration = null;
        switch (roleId)
        {
            case "1":
                trainingRegistration = _context.TrainingRegistration.Find(id);
                if (trainingRegistration == null) throw new KeyNotFoundException("Training registration not found");
                if (trainingRegistration.user_id != userId) throw new AppException("You do not have access to this registration");
                _context.TrainingRegistration.Remove(trainingRegistration);
                break;
            case "2":
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
                trainingRegistration = _context.TrainingRegistration.Include(tr => tr.User).Where(tr => tr.id == id).FirstOrDefault();
                if (trainingRegistration == null) throw new KeyNotFoundException("Training registration not found");
                if (trainingRegistration.User.team_id != team.id) throw new AppException("You do not have access to this registration");
                _context.TrainingRegistration.Remove(trainingRegistration);
                break;
            case "3":
                trainingRegistration = _context.TrainingRegistration.Find(id);
                if (trainingRegistration == null) throw new KeyNotFoundException("Training registration not found");
                _context.TrainingRegistration.Remove(trainingRegistration);
                break;
        }
        _context.SaveChanges();
    }

}