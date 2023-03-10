namespace WebApi.Entities;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Invite
{
    public int id { get; set; }

    [StringLength(50, MinimumLength = 5)]
    public string email { get; set; }

    [JsonIgnore, ForeignKey("Role")]
    public byte role_id {get;set;}
    public Role Role { get; set; }
    
    [JsonIgnore, ForeignKey("Team")]
    public byte? team_id {get;set;}
    public Team Team { get; set; }

    [StringLength(255, MinimumLength = 10)]
    public string token { get; set; }

    public bool user_created {get;set;}

}