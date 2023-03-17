namespace WebApi.Services;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Attendance;
using System;
using System.Text;
using AutoMapper;

public interface IAttendanceService
{
    IEnumerable<AttendanceView> GetAll(int currentUserId, string roleId, string teamId, int user_id, int training_id);
    void Create(CreateRequest model);
    Attendance GetById(int id);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}

public class AttendanceService : IAttendanceService
{
    private DataContext _context;
    private readonly IMapper _mapper;
    private ISharedService _sharedService;

    public AttendanceService(
        DataContext context,
        IMapper mapper)
    {
        _sharedService = new SharedService(context, mapper);
        _context = context;
        _mapper = mapper;
    }
    public IEnumerable<AttendanceView> GetAll(int currentUserId, string roleId, string teamId, int user_id, int training_id)
    {
        switch(roleId) {
            case "1":
                if(user_id == 0) user_id = currentUserId;
                else if(user_id != currentUserId) throw new AppException("You don't have access to this resource.");
                return _context.Attendance.Where(f => 
                f.user_id == user_id &&
                (training_id != 0 ? f.training_id == training_id: true))
                .Include(f => f.User).Include(f => f.Training).Include(f => f.Status).Select(f => new  AttendanceViewMax{
                    id=f.id,
                    User = new UserView {id = f.User.id,
                        username = f.User.username,
                        firstname = f.User.firstname,
                        lastname = f.User.lastname,
                        email = f.User.email},
                    Training = f.Training,
                    Status = f.Status,
                    start = f.start,
                    end = f.end,
                    user_id = f.user_id,
                    training_id = f.training_id,
                    status_id = f.status_id
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
                return _context.Attendance.Where(f => 
                (user_id != 0 ? f.user_id == user_id: true) &&
                (training_id != 0 ? f.training_id == training_id: true))
                .Include(f => f.User).Where(f => f.User.team_id == team.id).Include(f => f.Training).Include(f => f.Status).Select(f => new  AttendanceViewMax{
                    id=f.id,
                    User = new UserView {id = f.User.id,
                        username = f.User.username,
                        firstname = f.User.firstname,
                        lastname = f.User.lastname,
                        email = f.User.email},
                    Training = f.Training,
                    Status = f.Status,
                    start = f.start,
                    end = f.end,
                    user_id = f.user_id,
                    training_id = f.training_id,
                    status_id = f.status_id
                });
            case "3":
                return _context.Attendance.Where(f => 
                (user_id != 0 ? f.user_id == user_id: true) &&
                (training_id != 0 ? f.training_id == training_id: true))
                .Include(f => f.User).Include(f => f.Training).Include(f => f.Status).Select(f => new  AttendanceViewMax{
                    id=f.id,
                    User = new UserView {id = f.User.id,
                        username = f.User.username,
                        firstname = f.User.firstname,
                        lastname = f.User.lastname,
                        email = f.User.email},
                    Training = f.Training,
                    Status = f.Status,
                    start = f.start,
                    end = f.end,
                    user_id = f.user_id,
                    training_id = f.training_id,
                    status_id = f.status_id
                });
        }
        throw new AppException("Role not found");
    }

    public void Create(CreateRequest model)
    {
        var attendance = _mapper.Map<Attendance>(model);
        attendance.status_id = 1;
        _context.Attendance.Add(attendance);
        _context.SaveChanges();
    }

    public Attendance GetById(int id)
    {
        return _sharedService.GetAttendance(id);
    }

    public void Update(int id, UpdateRequest model)
    {
        var attendance = _sharedService.GetAttendance(id);
        _sharedService.GetAttendanceStatus(model.status_id);

        _mapper.Map(model, attendance);
        _context.Attendance.Update(attendance);
        _context.SaveChanges();
    }


    public void Delete(int id)
    {
        var attendance = _sharedService.GetAttendance(id);
        _context.Attendance.Remove(attendance);
        _context.SaveChanges();
    }

}