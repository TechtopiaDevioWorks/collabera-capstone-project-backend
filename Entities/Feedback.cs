namespace WebApi.Entities;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Feedback
{
    public int id { get; set; }

    [JsonIgnore, ForeignKey("FeedbackType")]
    public byte type_id { get; set; }
    public FeedbackType FeedbackType { get; set; }

    [JsonIgnore, ForeignKey("FromUser")]
    public int? from_user_id { get; set; }
    public User FromUser { get; set; }

    [JsonIgnore, ForeignKey("ToTraining")]
    public int? to_training_id { get; set; }
    public Training ToTraining {get;set;}

    [JsonIgnore, ForeignKey("ToAttendance")]
    public int? to_attendance_id { get; set; }
    public Attendance ToAttendance {get;set;}

    [JsonIgnore, ForeignKey("ToTrainingRegistration")]
    public int? to_training_registration_id { get; set; }
    public TrainingRegistration ToTrainingRegistration { get; set; }

    [JsonIgnore, ForeignKey("ToUser")]
    public int? to_user_id { get; set; }
    public User ToUser { get; set; }

    public string message {get;set;}
}
