namespace WebApi.Models.Feedback;
using System;
using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class CreateRequest
{

    [Required, Range(0, byte.MaxValue)]
    public byte type_id { get; set; }


    public int? from_user_id { get; set; }
    public int? to_training_id { get; set; }
    public int? to_attendance_id { get; set; }
    public int? to_training_registration_id { get; set; }
    public int? to_user_id { get; set; }
    public string message { get; set; }
}