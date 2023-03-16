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
    IEnumerable<FeedbackView> GetAll(int userId, string roleId, string teamId, int type_id, int from_user_id, int to_training_id, int to_attendance_id, int to_training_registration_id, int to_user_id);
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
    public IEnumerable<FeedbackView> GetAll(int userId, string roleId, string teamId, int type_id, int from_user_id, int to_training_id, int to_attendance_id, int to_training_registration_id, int to_user_id)
    {
        switch(roleId) {
            case "1":
                return _context.Feedback.Where(f => 
                f.type_id == type_id && 
                (from_user_id != 0 ? f.from_user_id == from_user_id: true) && 
                (to_training_id != 0 ?f.to_training_id == to_training_id: true) && 
                (to_attendance_id != 0 ? f.to_attendance_id == to_attendance_id: true) && 
                (to_training_registration_id != 0 ? f.to_training_registration_id == to_training_registration_id: true) && 
                (to_user_id != 0 ? f.to_user_id == to_user_id: true))
                .Where(f => f.from_user_id == userId || f.to_user_id == userId)
                .Include(f => f.FromUser).Include(f => f.ToUser).Include(f => f.ToTraining).Include(f => f.ToAttendance).Include(f => f.ToTrainingRegistration).Select(f => new FeedbackView {
                    id=f.id,
                    FeedbackType=f.FeedbackType,
                    FromUser = new UserView {id = f.FromUser.id,
                        username = f.FromUser.username,
                        firstname = f.FromUser.firstname,
                        lastname = f.FromUser.lastname,
                        email = f.FromUser.email},
                    ToUser = f.ToUser != null ? new UserView {id = f.ToUser.id,
                        username = f.ToUser.username,
                        firstname = f.ToUser.firstname,
                        lastname = f.ToUser.lastname,
                        email = f.ToUser.email} : null,
                    ToTraining = f.ToTraining,
                    ToAttendance = f.ToAttendance,
                    ToTrainingRegistration = f.ToTrainingRegistration,
                    message= f.message
                });
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
                return _context.Feedback.Where(f => 
                f.type_id == type_id && 
                (from_user_id != 0 ? f.from_user_id == from_user_id: true) && 
                (to_training_id != 0 ?f.to_training_id == to_training_id: true) && 
                (to_attendance_id != 0 ? f.to_attendance_id == to_attendance_id: true) && 
                (to_training_registration_id != 0 ? f.to_training_registration_id == to_training_registration_id: true) && 
                (to_user_id != 0 ? f.to_user_id == to_user_id: true))
                .Include(f => f.FromUser).Include(f => f.ToUser).Include(f => f.ToTraining).Include(f => f.ToAttendance).Include(f => f.ToTrainingRegistration)
                .Where(f => f.FromUser.team_id == team.id || f.ToUser.team_id == team.id)
                .Select(f => new FeedbackView {
                    id=f.id,
                    FeedbackType=f.FeedbackType,
                    FromUser = new UserView {id = f.FromUser.id,
                        username = f.FromUser.username,
                        firstname = f.FromUser.firstname,
                        lastname = f.FromUser.lastname,
                        email = f.FromUser.email},
                    ToUser = f.ToUser != null ? new UserView {id = f.ToUser.id,
                        username = f.ToUser.username,
                        firstname = f.ToUser.firstname,
                        lastname = f.ToUser.lastname,
                        email = f.ToUser.email} : null,
                    ToTraining = f.ToTraining,
                    ToAttendance = f.ToAttendance,
                    ToTrainingRegistration = f.ToTrainingRegistration,
                    message= f.message
                });
            case "3":
                return _context.Feedback.Where(f => 
                f.type_id == type_id && 
                (from_user_id != 0 ? f.from_user_id == from_user_id: true) && 
                (to_training_id != 0 ?f.to_training_id == to_training_id: true) && 
                (to_attendance_id != 0 ? f.to_attendance_id == to_attendance_id: true) && 
                (to_training_registration_id != 0 ? f.to_training_registration_id == to_training_registration_id: true) && 
                (to_user_id != 0 ? f.to_user_id == to_user_id: true))
                .Include(f => f.FromUser).Include(f => f.ToUser).Include(f => f.ToTraining).Include(f => f.ToAttendance).Include(f => f.ToTrainingRegistration).Select(f => new FeedbackView {
                    id=f.id,
                    FeedbackType=f.FeedbackType,
                    FromUser = new UserView {id = f.FromUser.id,
                        username = f.FromUser.username,
                        firstname = f.FromUser.firstname,
                        lastname = f.FromUser.lastname,
                        email = f.FromUser.email},
                    ToUser = f.ToUser != null ? new UserView {id = f.ToUser.id,
                        username = f.ToUser.username,
                        firstname = f.ToUser.firstname,
                        lastname = f.ToUser.lastname,
                        email = f.ToUser.email} : null,
                    ToTraining = f.ToTraining,
                    ToAttendance = f.ToAttendance,
                    ToTrainingRegistration = f.ToTrainingRegistration,
                    message= f.message
                });
        }
        throw new AppException("Role not found");
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