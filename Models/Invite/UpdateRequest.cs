namespace WebApi.Models.Invite;

using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

public class UpdateRequest
{
    [Required]
    public byte role_id { get; set; }

    [CustomAnnotations.RequiredIfNot("role_id", (byte)3)]
    public byte? team_id { get; set; }
 
}