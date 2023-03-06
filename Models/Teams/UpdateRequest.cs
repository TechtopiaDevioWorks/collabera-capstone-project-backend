namespace WebApi.Models.Teams;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class UpdateRequest
{
    [Required]
    public string name { get; set; }
 
}