namespace WebApi.Entities;

using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public int id { get; set; }
    public string username { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string email { get; set; }

    [JsonIgnore, ForeignKey("Role")]
    public byte role_id {get;set;}
    
    [JsonIgnore, ForeignKey("Team")]
    public byte? team_id {get;set;}

    [JsonIgnore]
    public string password { get; set; }

    [JsonIgnore]
    public string token { get; set; }

    public Role Role { get; set; }
    public Team Team { get; set; }
}