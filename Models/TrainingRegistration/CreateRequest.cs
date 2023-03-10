namespace WebApi.Models.TrainingRegistration;

using System;
using System.ComponentModel.DataAnnotations;
using WebApi.Entities;
using CustomAnnotations;

public class CreateRequest
{
    [Required, Range(0, Int32.MaxValue)]
    public int user_id { get; set; }
    [Required, Range(0, Int32.MaxValue)]
    public int training_id { get; set; }
}