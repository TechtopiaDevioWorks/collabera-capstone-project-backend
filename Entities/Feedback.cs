namespace WebApi.Entities;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Feedback
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(1, int.MaxValue)]
    public int id { get; set; }

    [ForeignKey("FeedbackType")]
    public byte type_id { get; set; }
    public FeedbackType FeedbackType { get; set; }

    [ForeignKey("FromUser")]
    public int? from_user_id { get; set; }
    public User FromUser { get; set; }

    [ForeignKey("ToTraining")]
    public int? to_training_id { get; set; }
    public Training ToTraining {get;set;}

    [ForeignKey("ToAttendance")]
    public int? to_attendance_id { get; set; }
    public Attendance ToAttendance {get;set;}

    [ForeignKey("ToTrainingRegistration")]
    public int? to_training_registration_id { get; set; }
    public TrainingRegistration ToTrainingRegistration { get; set; }

    [ForeignKey("ToUser")]
    public int? to_user_id { get; set; }
    public User ToUser { get; set; }

    public string message {get;set;}
}
public class FeedbackView {
    public int id { get; set; }
    public FeedbackType FeedbackType { get; set; }
    public UserView FromUser { get; set; }
    public Training ToTraining {get;set;}
    public Attendance ToAttendance {get;set;}
    public TrainingRegistration ToTrainingRegistration { get; set; }
    public UserView ToUser { get; set; }
    public string message {get;set;}
}