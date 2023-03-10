namespace WebApi.Entities;
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Training
{
    public int id { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string name { get; set; }

    [StringLength(255, MinimumLength = 1)]
    public string description { get; set; }

    [Required]
    public DateTime start { get; set; }

    [Required]
    public DateTime end { get; set; }

    [Required]
    public int min_hours { get; set; }

    [JsonIgnore, ForeignKey("Status")]
    public byte status_id { get; set; }
    public TrainingStatus Status { get; set; }
}