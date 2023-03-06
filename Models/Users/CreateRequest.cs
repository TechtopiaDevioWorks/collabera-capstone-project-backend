namespace WebApi.Models.Users;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class CreateRequest
{
    [Required]
    public string username { get; set; }

    [Required]
    public string firstname { get; set; }

    [Required]
    public string lastname { get; set; }

    [Required]
    public string role_id { get; set; }

    [Required]
    [EmailAddress]
    public string email { get; set; }

    [Required]
    [MinLength(6)]
    public string password { get; set; }
}