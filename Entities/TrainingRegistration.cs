namespace WebApi.Entities;
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TrainingRegistration
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(1, int.MaxValue)]
    public int id { get; set; }

    [ForeignKey("User")]
    public int user_id { get; set; }
    public User User { get; set; }

    [ForeignKey("Training")]
    public int training_id { get; set; }
    public Training Training { get; set; }

    public DateTime registration_date { get; set; }

    [ForeignKey("Status")]
    public byte status_id { get; set; }
    public TrainingRegistrationStatus Status { get; set; }
}

public class TrainingRegistrationView {
    public int id { get; set; }
    public int user_id { get; set; }
    public int training_id { get; set; }
    public DateTime registration_date { get; set; }
    public byte status_id { get; set; }
}

public class TrainingRegistrationViewMax: TrainingRegistrationView {
    public UserView User { get; set; }
    public Training Training { get; set; }
    public TrainingRegistrationStatus Status { get; set; }
}

public class TrainingRegistrationViewMaxExpand: TrainingRegistrationView {
    public UserViewExpand User { get; set; }
    public Training Training { get; set; }
    public TrainingRegistrationStatus Status { get; set; }
}