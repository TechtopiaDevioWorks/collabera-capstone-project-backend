namespace WebApi.Entities;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Team
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(1, byte.MaxValue)]
    public byte id { get; set; }

    [StringLength(25, MinimumLength = 1)]
    public string name { get; set; }

    [JsonIgnore]
    public ICollection<User> Users { get; set; }

    [JsonIgnore]
    public ICollection<Invite> Invites { get; set; }

}

public class TeamView
{
    public byte id { get; set; }
    public string name { get; set; }
    public ICollection<UserView> Users { get; set; }
    public ICollection<Invite> Invites { get; set; }
}