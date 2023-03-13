namespace WebApi.Services;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

public interface ISharedService
{
    Role GetRole(byte id);
    Team GetTeam(byte? id);
    TeamView GetTeamExpand(byte? id);
    User GetUser(int id);
    Invite GetInvite(int id);
    Training GetTraining(int id);
    Attendance GetAttendance(int id);
    Feedback GetFeedback(int id);
    string GenerateToken();
    TrainingRegistration GetTrainingRegistration(int id);
    TrainingRegistrationStatus GetTrainingRegistrationStatus(byte? id);
    TrainingStatus GetTrainingStatus(byte? id);
    FeedbackType GetFeedbackType(byte? id);
    AttendanceStatus GetAttendanceStatus(byte? id);
}

public class SharedService : ISharedService
{
    private DataContext _context;
    private readonly IMapper _mapper;

    public SharedService(
        DataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    // helper methods
    public TrainingRegistrationStatus GetTrainingRegistrationStatus(byte? id)
    {
        var status = _context.TrainingRegistrationStatus.Find(id);
        if (status == null) throw new KeyNotFoundException("Status not found");
        return status;
    }

    public TrainingStatus GetTrainingStatus(byte? id)
    {
        var status = _context.TrainingStatus.Find(id);
        if (status == null) throw new KeyNotFoundException("Status not found");
        return status;
    }

    public AttendanceStatus GetAttendanceStatus(byte? id)
    {
        var status = _context.AttendanceStatus.Find(id);
        if (status == null) throw new KeyNotFoundException("Status not found");
        return status;
    }

    public FeedbackType GetFeedbackType(byte? id)
    {
        var feedbackType = _context.FeedbackType.Find(id);
        if (feedbackType == null) throw new KeyNotFoundException("Feedback type not found");
        return feedbackType;
    }


    public Invite GetInvite(int id)
    {
        var invite = _context.Invite.Find(id);
        if (invite == null) throw new KeyNotFoundException("Invite not found");
        return invite;
    }

    public Feedback GetFeedback(int id)
    {
        var feedback = _context.Feedback.Find(id);
        if (feedback == null) throw new KeyNotFoundException("Feedback not found");
        return feedback;
    }

    public Attendance GetAttendance(int id)
    {
        var attendance = _context.Attendance.Find(id);
        if (attendance == null) throw new KeyNotFoundException("Attendance not found");
        return attendance;
    }

    public TrainingRegistration GetTrainingRegistration(int id)
    {
        var trainingRegistration = _context.TrainingRegistration.Find(id);
        if (trainingRegistration == null) throw new KeyNotFoundException("Training registration not found");
        return trainingRegistration;
    }

    public Training GetTraining(int id)
    {
        var training = _context.Training.Include(t => t.Status).FirstOrDefault(t => t.id == id);
        if (training == null) throw new KeyNotFoundException("Training not found");
        return training;
    }

    public Role GetRole(byte id)
    {
        var role = _context.Role.Find(id);
        if (role == null) throw new KeyNotFoundException("Role not found");
        return role;
    }

    public Team GetTeam(byte? id)
    {
        var team = _context.Team.Find(id);
        if (team == null) throw new KeyNotFoundException("Team not found");
        return team;
    }

    public TeamView GetTeamExpand(byte? id)
    {
        var team = _context.Team.Include(t => t.Users).Include(t => t.Invites).FirstOrDefault(t => t.id == id);
        if (team == null) throw new KeyNotFoundException("Team not found");
        var teamView = new TeamView
        {
            id = team.id,
            name = team.name,
            Users = team.Users.Select(u => new UserView
            {
                id = u.id,
                username = u.username,
                firstname = u.firstname,
                lastname = u.lastname,
                email = u.email,
            }).ToList(),
            Invites = team.Invites
        };
        if (teamView == null) throw new KeyNotFoundException("Team not found");
        return teamView;
    }

    public User GetUser(int id)
    {
        var user = _context.User.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    public string GenerateToken()
    {
        string guid = Guid.NewGuid().ToString();
        byte[] guidBytes = Encoding.UTF8.GetBytes(guid);
        using (var sha512 = SHA512.Create())
        {
            byte[] hashBytes = sha512.ComputeHash(guidBytes);
            string hashedToken = Base64UrlEncoder.Encode(Convert.ToBase64String(hashBytes));
            return hashedToken;
        }
    }

}