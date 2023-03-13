namespace WebApi.Models.TrainingRegistration;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class UpdateRequest
{
    [Required, Range(1, byte.MaxValue)]
    public byte status_id { get; set; }
}