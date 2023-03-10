namespace WebApi.Entities;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TrainingRegistrationStatus
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(1, byte.MaxValue)]
    public byte id { get; set; }

    [StringLength(25, MinimumLength = 1)]
    public string name { get; set; }

    [JsonIgnore]
    public ICollection<TrainingRegistration> TrainingRegistrations { get; set; }

}