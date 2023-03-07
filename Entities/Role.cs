namespace WebApi.Entities;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Role
{
    public byte id { get; set; }
    public string name { get; set; }
    
    [JsonIgnore]
    public ICollection<User> Users { get; set; }
}