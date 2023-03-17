namespace WebApi.Entities;
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Attendance
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Range(1, int.MaxValue)]
    public int id { get; set; }

    [ ForeignKey("User")]
    public int user_id { get; set; }
    public User User { get; set; }

    [ ForeignKey("Training")]
    public int training_id { get; set; }
    public Training Training { get; set; }

    public DateTime start { get; set; }
    public DateTime end { get; set; }

    [ ForeignKey("Status")]
    public byte status_id { get; set; }
    public AttendanceStatus Status { get; set; }
}

public class AttendanceView {
    public int id { get; set; }
    public int user_id { get; set; }
    public int training_id { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public byte status_id { get; set; }
}

public class AttendanceViewMax : AttendanceView {
    public UserView User { get; set; }
    public Training Training { get; set; }
    public AttendanceStatus Status { get; set; }
}