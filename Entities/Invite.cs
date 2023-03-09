namespace WebApi.Entities;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Invite
{
    public int id { get; set; }
    public string email { get; set; }

    [JsonIgnore, ForeignKey("Role")]
    public byte role_id {get;set;}
    
    [JsonIgnore, ForeignKey("Team")]
    public byte? team_id {get;set;}
    public string token { get; set; }

    public Role Role { get; set; }
    public Team Team { get; set; }
}