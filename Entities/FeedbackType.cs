namespace WebApi.Entities;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class FeedbackType
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(1, byte.MaxValue)]
    public byte id { get; set; }

    [StringLength(35, MinimumLength = 1)]
    public string name { get; set; }

    [JsonIgnore]
    public ICollection<Feedback> Feedbacks { get; set; }

}