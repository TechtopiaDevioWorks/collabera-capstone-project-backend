namespace WebApi.Entities;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Team
{
    public byte id { get; set; }
    public string name { get; set; }
    

    public ICollection<User> Users { get; set; }

    public ICollection<Invite> Invites { get; set; }
}