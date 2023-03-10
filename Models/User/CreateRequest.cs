namespace WebApi.Models.User;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class RegisterRequest
{
    [Required, MinLength(5), MaxLength(50)]
    public string username { get; set; }

    [Required, MaxLength(50)]
    public string firstname { get; set; }

    [Required, MaxLength(50)]
    public string lastname { get; set; }

    [Required, MaxLength(255)]
    public string token {get;set;}

    [Required, MaxLength(50)]
    [EmailAddress]
    public string email { get; set; }

    [Required, MinLength(5), MaxLength(50)]
    public string password { get; set; }
}

public class CreateRequest: RegisterRequest
{
    [Required]
    public byte role_id { get; set; }
    [CustomAnnotations.RequiredIfNot("role_id", (byte)3)]
    public byte? team_id { get; set; }
}