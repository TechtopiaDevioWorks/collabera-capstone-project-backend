namespace WebApi.Services;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.User;
using System;
using System.Text;
using AutoMapper;
using BCrypt.Net;

public interface IUserService
{
    IEnumerable<UserView> GetAll(Boolean expand = false);
    User Login(LoginRequest model);
    User GetById(int id);
    void Create(RegisterRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}

public class UserService : IUserService
{
    private DataContext _context;
    private readonly IMapper _mapper;
    private ISharedService _sharedService;
    public UserService(
        DataContext context,
        IMapper mapper)
    {
        _sharedService = new SharedService(context, mapper);
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<UserView> GetAll(Boolean expand = false)
    {
        if (expand == true)
            return _context.User.Include(u => u.Role).Include(u => u.Team).Select(u => new UserView
            {
                id = u.id,
                username = u.username,
                firstname = u.firstname,
                lastname = u.lastname,
                email = u.email,
                Team = u.Team,
                Role = u.Role
            });
        else
            return _context.User.Select(u => new UserView
            {
                id = u.id,
                username = u.username,
                firstname = u.firstname,
                lastname = u.lastname,
                email = u.email,
            });
    }

    public User GetById(int id)
    {
        return _sharedService.getUser(id);
    }

    public User Login(LoginRequest model)
    {
        var user = _context.User.Include(u => u.Role).Include(u => u.Team).Where(x => x.username == model.username).FirstOrDefault();
        if (user == null) throw new KeyNotFoundException("Invalid username and/or password.");
        var passwordCheck = BCrypt.EnhancedVerify(model.password, user.password);
        if (passwordCheck == false) throw new KeyNotFoundException("Invalid username and / or password.");
        else
        {
            user.token = _sharedService.GenerateToken();
            _context.User.Update(user);
            _context.SaveChanges();
            return user;
        }
    }

    public void Create(RegisterRequest model)
    {
        // validate
        if (_context.User.Any(x => x.email == model.email))
            throw new AppException("User with the email '" + model.email + "' already exists");
        if (_context.User.Any(x => x.username == model.username))
            throw new AppException("User with the username '" + model.username + "' already exists");
        if (!_context.Invite.Any(x => x.email == model.email))
            throw new AppException("Email '" + model.email + "' has not been invited. Ask HR for a invitation.");
        var invite = _context.Invite.Where(x => x.email == model.email && x.token == model.token).FirstOrDefault();
        if (invite == null)
            throw new AppException("Your invite token is not valid. Try again or ask HR for assistance.");
        // map model to new user object
        CreateRequest cmodel = new CreateRequest()
        {
            username = model.username,
            firstname = model.firstname,
            lastname = model.lastname,
            email = model.email,
            password = BCrypt.EnhancedHashPassword(model.password),
            role_id = invite.role_id,
            team_id = invite.team_id,
            token = _sharedService.GenerateToken()
        };
        var user = _mapper.Map<User>(cmodel);
        invite.user_created = true;
        _context.Invite.Update(invite);
        // save user
        _context.User.Add(user);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateRequest model)
    {
        var user = _sharedService.getUser(id);

        // validate
        if (model.email != user.email && _context.User.Any(x => x.email == model.email))
            throw new AppException("User with the email '" + model.email + "' already exists");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.password))
            user.password = BCrypt.EnhancedHashPassword(model.password);

        // copy model to user and save
        _mapper.Map(model, user);
        _context.User.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = _sharedService.getUser(id);
        _context.User.Remove(user);
        _context.SaveChanges();
    }

    // helper methods

}