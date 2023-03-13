namespace WebApi.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BCrypt.Net;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Invite;
using System;
using System.Security.Cryptography;
using System.Text;


public interface IInviteService
{
    IEnumerable<Invite> GetAll();
    Invite GetById(int id);
    void Create(CreateRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}

public class InviteService : IInviteService
{
    private DataContext _context;
    private readonly IMapper _mapper;
    private ISharedService _sharedService;
    public InviteService(
        DataContext context,
        IMapper mapper)
    {
        _sharedService = new SharedService(context, mapper);
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<Invite> GetAll()
    {
        return _context.Invite.Include(i => i.Team).Include(i => i.Role);
    }

    public Invite GetById(int id)
    {
        return _sharedService.GetInvite(id);
    }

    public void Create(CreateRequest model)
    {
        // validate
        _sharedService.GetRole(model.role_id);
        if(model.role_id != 3) {
            if (model.team_id == null) throw new AppException("Team is required for non HR members");
            else {
                _sharedService.GetTeam(model.team_id);
            }
        }
        if (_context.Invite.Any(x => x.email == model.email))
            throw new AppException("Invite with the email '" + model.email + "' already exists");
        // map model to new user object
        var invite = _mapper.Map<Invite>(model);
        invite.token = _sharedService.GenerateToken();
        invite.user_created = false;
        // save user
        _context.Invite.Add(invite);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateRequest model)
    {
        var invite = _sharedService.GetInvite(id);
        if(invite.user_created == true) throw new AppException("Invite was already accepted, can not change it anymore.");
        _sharedService.GetRole(model.role_id);
        if(model.role_id != 3) {
            if (model.team_id == null) throw new AppException("Team is required for non HR members");
            else {
                _sharedService.GetTeam(model.team_id);
            }
        }

        // copy model to invite and save
        _mapper.Map(model, invite);
        invite.token = _sharedService.GenerateToken();
        _context.Invite.Update(invite);
        _context.SaveChanges();
    }



    public void Delete(int id)
    {
        var invite = _sharedService.GetInvite(id);
        _context.Invite.Remove(invite);
        _context.SaveChanges();
    }

}