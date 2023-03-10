namespace WebApi.Models.User;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class UpdateRequest
{
    [Required, MinLength(5), MaxLength(50)]
    public string username { get; set; }
    [Required, MaxLength(50)]
    public string firstname { get; set; }
    [Required, MaxLength(50)]
    public string lastname { get; set; }

    public byte role_id { get; set; }
    public byte team_id { get; set; }
    [EmailAddress, MaxLength(50)]
    public string email { get; set; }

    // treat empty string as null for password fields to 
    // make them optional in front end apps
    private string _password;
    [MinLength(5)]
    public string password
    {
        get => _password;
        set => _password = replaceEmptyWithNull(value);
    }
    // helpers

    private string replaceEmptyWithNull(string value)
    {
        // replace empty string with null to make field optional
        return string.IsNullOrEmpty(value) ? null : value;
    }
}

public class LoginRequest {
    [Required, MinLength(5), MaxLength(50)]
    public string username { get; set; }

    [Required, MinLength(5), MaxLength(50)]
    public string password { get; set; }
}