namespace WebApi.Models.Users;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class UpdateRequest
{
    public string username { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }

    public byte role_id { get; set; }
    public byte team_id { get; set; }
    [EmailAddress]
    public string email { get; set; }

    // treat empty string as null for password fields to 
    // make them optional in front end apps
    private string _password;
    [MinLength(6)]
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