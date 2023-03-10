namespace WebApi.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BCrypt.Net;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Team;

public interface ITeamService
{
    IEnumerable<Team> GetAll();
    Team GetById(byte id);
    void Create(CreateRequest model);
    void Update(byte id, UpdateRequest model);
    void Delete(byte id);
}

public class TeamService : ITeamService
{
    private DataContext _context;
    private readonly IMapper _mapper;
    private ISharedService _sharedService;
    public TeamService(
        DataContext context,
        IMapper mapper)
    {
        _sharedService = new SharedService(context, mapper);
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<Team> GetAll()
    {
        return _context.Team
            .Include(team => team.Users)
                .ThenInclude(user => user.Role)
            .Include(team => team.Invites)
                .ThenInclude(invite => invite.Role);
    }

    public Team GetById(byte id)
    {
        return _sharedService.getTeam(id);
    }

    public void Create(CreateRequest model)
    {
        // validate
        if (_context.Team.Any(x => x.name == model.name))
            throw new AppException("Team with the name '" + model.name + "' already exists");
        // map model to new user object
        var team = _mapper.Map<Team>(model);
        // save user
        _context.Team.Add(team);
        _context.SaveChanges();
    }

    public void Update(byte id, UpdateRequest model)
    {
        var team = _sharedService.getTeam(id);

        // validate
        if (model.name != team.name && _context.Team.Any(x => x.name == model.name))
            throw new AppException("Team with the name '" + model.name + "' already exists");

        // copy model to user and save
        _mapper.Map(model, team);
        _context.Team.Update(team);
        _context.SaveChanges();
    }

    public void Delete(byte id)
    {
        var team = _sharedService.getTeam(id);
        _context.Team.Remove(team);
        _context.SaveChanges();
    }

}