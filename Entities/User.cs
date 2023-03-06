namespace WebApi.Entities;

using System.Text.Json.Serialization;

public class User
{
    public int id { get; set; }
    public string username { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string email { get; set; }
    public Role role { get; set; }
    public Team team { get; set; } = default!;

    [JsonIgnore]
    public string password { get; set; }

    [JsonIgnore]
    public string token { get; set; }
}