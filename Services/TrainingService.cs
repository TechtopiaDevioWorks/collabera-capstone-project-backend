namespace WebApi.Services;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Training;
using System;
using System.Text;
using AutoMapper;

public interface ITrainingService
{
    IEnumerable<Training> GetAll(Boolean expand = false);
    void Create(CreateRequest model);
    Training GetById(int id);
    void Delete(int id);
}

public class TrainingService : ITrainingService
{
    private DataContext _context;
    private readonly IMapper _mapper;
    private ISharedService _sharedService;

    public TrainingService(
        DataContext context,
        IMapper mapper)
    {
        _sharedService = new SharedService(context, mapper);
        _context = context;
        _mapper = mapper;
    }
    public IEnumerable<Training> GetAll(Boolean expand = false)
    {
        if (expand == true)
            return _context.Training;
        else
            return _context.Training;
    }

    public void Create(CreateRequest model)
    {
        var training = _mapper.Map<Training>(model);
        training.status_id = 1;
        _context.Training.Add(training);
        _context.SaveChanges();
    }

    public Training GetById(int id)
    {
        return _sharedService.GetTraining(id);
    }

    public void Delete(int id)
    {
        var training = _sharedService.GetTraining(id);
        _context.Training.Remove(training);
        _context.SaveChanges();
    }

}