namespace WebApi.Entities;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public int id { get; set; }

    [StringLength(50, MinimumLength = 5)]
    public string username { get; set; }

    [StringLength(50, MinimumLength = 5)]
    public string firstname { get; set; }

    [StringLength(50, MinimumLength = 5)]
    public string lastname { get; set; }

    [StringLength(50, MinimumLength = 5)]
    public string email { get; set; }

    [JsonIgnore, ForeignKey("Role")]
    public byte role_id { get; set; }
    public Role Role { get; set; }

    [JsonIgnore, ForeignKey("Team")]
    public byte? team_id { get; set; }
    public Team Team { get; set; }

    [JsonIgnore, StringLength(255, MinimumLength = 5)]
    public string password { get; set; }
    [StringLength(255, MinimumLength = 5)]
    public string token { get; set; }

}

public class UserView
{
    public int id { get; set; }

    public string username { get; set; }

    public string firstname { get; set; }

    public string lastname { get; set; }

    public string email { get; set; }

    public Role Role { get; set; }

    public Team Team { get; set; }
}
