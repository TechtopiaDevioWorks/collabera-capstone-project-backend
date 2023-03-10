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
    Role getRole(byte id);
    Team getTeam(byte? id);
    User getUser(int id);
    Training GetTraining(int id);
    Attendance GetAttendance(int id);
    Feedback GetFeedback(int id);
    string GenerateToken();
    TrainingRegistration GetTrainingRegistration(int id);
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
        var training = _context.Training.Find(id);
        if (training == null) throw new KeyNotFoundException("Training not found");
        return training;
    }

    public Role getRole(byte id)
    {
        var role = _context.Role.Find(id);
        if (role == null) throw new KeyNotFoundException("Role not found");
        return role;
    }

    public Team getTeam(byte? id)
    {
        var team = _context.Team.Find(id);
        if (team == null) throw new KeyNotFoundException("Team not found");
        return team;
    }

    public User getUser(int id)
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