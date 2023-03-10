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
    IEnumerable<Attendance> GetAll(Boolean expand = false);
    void Create(CreateRequest model);
    Attendance GetById(int id);
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
    public IEnumerable<Attendance> GetAll(Boolean expand = false)
    {
        if (expand == true)
            return _context.Attendance;
        else
            return _context.Attendance;
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

    public void Delete(int id)
    {
        var attendance = _sharedService.GetAttendance(id);
        _context.Attendance.Remove(attendance);
        _context.SaveChanges();
    }

}