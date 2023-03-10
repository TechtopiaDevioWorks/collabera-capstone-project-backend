namespace WebApi.Models.Training;

using System;
using System.ComponentModel.DataAnnotations;
using WebApi.Entities;
using CustomAnnotations;

public class CreateRequest
{
    [Required]
    public string name { get; set; }

    [Required]
    public string description { get; set; }

    [Required, DateTimeAfterCurrentDateAttribute]
    public DateTime start { get; set; }

    [Required, DateTimeAfter("start")]
    public DateTime end { get; set; }

    [Required, Range(1,1000)]
    public int min_hours { get; set; }
    
}