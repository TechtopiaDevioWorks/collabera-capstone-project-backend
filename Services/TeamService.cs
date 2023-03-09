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
    Team GetById(int id);
    void Create(CreateRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}

public class TeamService : ITeamService
{
    private DataContext _context;
    private readonly IMapper _mapper;

    public TeamService(
        DataContext context,
        IMapper mapper)
    {
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

    public Team GetById(int id)
    {
        return getTeam(id);
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

    public void Update(int id, UpdateRequest model)
    {
        var team = getTeam(id);

        // validate
        if (model.name != team.name && _context.Team.Any(x => x.name == model.name))
            throw new AppException("Team with the name '" + model.name + "' already exists");

        // copy model to user and save
        _mapper.Map(model, team);
        _context.Team.Update(team);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var team = getTeam(id);
        _context.Team.Remove(team);
        _context.SaveChanges();
    }

    // helper methods

    private Team getTeam(int id)
    {
        var team = _context.Team.Find(id);
        if (team == null) throw new KeyNotFoundException("Team not found");
        return team;
    }

}