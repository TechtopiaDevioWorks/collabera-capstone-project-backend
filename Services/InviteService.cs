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
    void Delete(int id);
}

public class InviteService : IInviteService
{
    private DataContext _context;
    private readonly IMapper _mapper;

    public InviteService(
        DataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<Invite> GetAll()
    {
        return _context.Invite;
    }

    public Invite GetById(int id)
    {
        return getInvite(id);
    }

    public void Create(CreateRequest model)
    {
        // validate
        if (_context.Invite.Any(x => x.email == model.email))
            throw new AppException("Invite with the email '" + model.email + "' already exists");
        // map model to new user object
        var invite = _mapper.Map<Invite>(model);
        // save user
        _context.Invite.Add(invite);
        _context.SaveChanges();
    }


    public void Delete(int id)
    {
        var invite = getInvite(id);
        _context.Invite.Remove(invite);
        _context.SaveChanges();
    }

    // helper methods

    private Invite getInvite(int id)
    {
        var invite = _context.Invite.Find(id);
        if (invite == null) throw new KeyNotFoundException("Invite not found");
        return invite;
    }
}