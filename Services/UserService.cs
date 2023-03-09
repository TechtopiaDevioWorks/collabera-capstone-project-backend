namespace WebApi.Services;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.User;
using System;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using BCrypt.Net;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User GetById(int id);
    void Create(RegisterRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}

public class UserService : IUserService
{
    private DataContext _context;
    private readonly IMapper _mapper;

    public UserService(
        DataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.User.Include(u => u.Role).Include(u => u.Team);
    }

    public User GetById(int id)
    {
        return getUser(id);
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
        CreateRequest cmodel = new CreateRequest(){
            username=model.username,
            firstname=model.firstname,
            lastname=model.lastname,
            email=model.email,
            password = BCrypt.HashPassword(model.password),
            role_id=invite.role_id,
            team_id=invite.team_id,
        };
        var user = _mapper.Map<User>(cmodel);

        // save user
        _context.User.Add(user);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateRequest model)
    {
        var user = getUser(id);

        // validate
        if (model.email != user.email && _context.User.Any(x => x.email == model.email))
            throw new AppException("User with the email '" + model.email + "' already exists");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.password))
            user.password = BCrypt.HashPassword(model.password);

        // copy model to user and save
        _mapper.Map(model, user);
        _context.User.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = getUser(id);
        _context.User.Remove(user);
        _context.SaveChanges();
    }

    // helper methods

    private User getUser(int id)
    {
        var user = _context.User.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
    private static string GenerateToken()
    {
        string guid = Guid.NewGuid().ToString();
        byte[] guidBytes = Encoding.UTF8.GetBytes(guid);
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(guidBytes);
            string hashedToken = Convert.ToBase64String(hashBytes);
            return hashedToken;
        }
    }
}