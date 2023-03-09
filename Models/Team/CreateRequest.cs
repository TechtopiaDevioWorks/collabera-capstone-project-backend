namespace WebApi.Models.Team;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class CreateRequest
{
    [Required]
    public string name { get; set; }
}