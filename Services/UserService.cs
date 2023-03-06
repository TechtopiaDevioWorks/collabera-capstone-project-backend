namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User GetById(int id);
    void Create(CreateRequest model);
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
        return _context.Users;
    }

    public User GetById(int id)
    {
        return getUser(id);
    }

    public void Create(CreateRequest model)
    {
        // validate
        if (_context.Users.Any(x => x.email == model.email))
            throw new AppException("User with the email '" + model.email + "' already exists");
        if (_context.Users.Any(x => x.username == model.username))
            throw new AppException("User with the username '" + model.username + "' already exists");
        // map model to new user object
        var user = _mapper.Map<User>(model);

        // hash password
        user.password = BCrypt.HashPassword(model.password);

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateRequest model)
    {
        var user = getUser(id);

        // validate
        if (model.email != user.email && _context.Users.Any(x => x.email == model.email))
            throw new AppException("User with the email '" + model.email + "' already exists");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.password))
            user.password = BCrypt.HashPassword(model.password);

        // copy model to user and save
        _mapper.Map(model, user);
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = getUser(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    // helper methods

    private User getUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
}