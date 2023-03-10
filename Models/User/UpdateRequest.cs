namespace WebApi.Models.User;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class UpdateRequest
{
    private string _username;
    private string _firstname;
    private string _lastname;
    private byte _role_id;
    private byte _team_id;
    private string _email;

    [MinLength(5), MaxLength(50)]
    public string username
    {
        get => _username;
        set => _username = replaceEmptyWithNull(value);
    }

    [MaxLength(50)]
    public string firstname
    {
        get => _firstname;
        set => _firstname = replaceEmptyWithNull(value);
    }

    [MaxLength(50)]
    public string lastname
    {
        get => _lastname;
        set => _lastname = replaceEmptyWithNull(value);
    }

    public byte role_id
    {
        get => _role_id;
        set => _role_id = value;
    }

    public byte team_id
    {
        get => _team_id;
        set => _team_id = value;
    }

    [EmailAddress, MaxLength(50)]
    public string email
    {
        get => _email;
        set => _email = replaceEmptyWithNull(value);
    }

    private string _password;
    [MinLength(5), MaxLength(50)]
    public string password
    {
        get => _password;
        set => _password = replaceEmptyWithNull(value);
    }

    private string replaceEmptyWithNull(string value)
    {
        // replace empty string with null to make field optional
        return string.IsNullOrEmpty(value) ? null : value;
    }
}

public class LoginRequest
{
    [Required, MinLength(5), MaxLength(50)]
    public string username { get; set; }

    [Required, MinLength(5), MaxLength(50)]
    public string password { get; set; }
}