namespace WebApi.Models.Attendance;
using System;
using System.ComponentModel.DataAnnotations;
using CustomAnnotations;
using WebApi.Entities;

public class CreateRequest
{

    [Required, Range(1, Int32.MaxValue)]
    public int user_id { get; set; }

    [Required, Range(1, Int32.MaxValue)]
    public int training_id { get; set; }

    [Required, DateTimeAfterCurrentDateAttribute]
    public DateTime start { get; set; }

    [Required, DateTimeAfter("start")]
    public DateTime end { get; set; }

    public byte status_id { get; set; }

}