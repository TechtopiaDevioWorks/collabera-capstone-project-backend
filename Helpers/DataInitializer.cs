using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
public static class DbInitializer
{
    public static void Initialize(DataContext context)
    {
        //create the database
        context.Database.EnsureCreated();

        InitRoles(context);
        InitTrainingRegistrationStatus(context);
        InitTrainingStatus(context);
        InitAttendanceStatus(context);
        InitFeedbackType(context);
        context.SaveChanges();
        context.Database.Migrate();
    }

    public static void InitRoles(DataContext context)
    {
        // Look for any roles.
        if (context.Role.Any())
        {
            return;   // DB has data
        }

        var roles = new WebApi.Entities.Role[]
        {
        new WebApi.Entities.Role{id=1,name="Employee"},
        new WebApi.Entities.Role{id=2,name="Manager"},
        new WebApi.Entities.Role{id=3,name="HR"},
        };

        foreach (WebApi.Entities.Role s in roles)
        {
            context.Role.Add(s);
        }
        if (context.User.Any())
        {
            return;
        }
        context.User.Add(new WebApi.Entities.User { id = 1, username = "admin", firstname = "admin", lastname = "admin", email = "admin@admin.adm", token = "default", password = BCrypt.Net.BCrypt.EnhancedHashPassword("admin"), Role = roles[2] });

    }

    public static void InitTrainingRegistrationStatus(DataContext context)
    {
        // Look for any training registration status.
        if (context.TrainingRegistrationStatus.Any())
        {
            return;   // DB has data
        }
        var TrainingRegistrationStatus = new WebApi.Entities.TrainingRegistrationStatus[] {
            new WebApi.Entities.TrainingRegistrationStatus{id=1, name="Applied"},
            new WebApi.Entities.TrainingRegistrationStatus{id=2, name="Rejected"},
            new WebApi.Entities.TrainingRegistrationStatus{id=3, name="Approved"},
        };

        foreach (var s in TrainingRegistrationStatus)
        {
            context.TrainingRegistrationStatus.Add(s);
        }

    }

    public static void InitTrainingStatus(DataContext context)
    {
        //Look for any training status
        if (context.TrainingStatus.Any())
        {
            return;
        }

        var TrainingStatus = new WebApi.Entities.TrainingStatus[] {
            new WebApi.Entities.TrainingStatus{id=1, name="Available"},
            new WebApi.Entities.TrainingStatus{id=2, name="Disabled"},
            new WebApi.Entities.TrainingStatus{id=3, name="Pending"}
        };

        foreach (var s in TrainingStatus)
        {
            context.TrainingStatus.Add(s);
        }
    }
    public static void InitAttendanceStatus(DataContext context)
    {
        //Look for any attendance status
        if (context.AttendanceStatus.Any())
        {
            return;
        }

        var AttendanceStatus = new WebApi.Entities.AttendanceStatus[] {
            new WebApi.Entities.AttendanceStatus{id=1, name="Submitted"},
            new WebApi.Entities.AttendanceStatus{id=2, name="Rejected"},
            new WebApi.Entities.AttendanceStatus{id=3, name="Approved"}
        };

        foreach (var s in AttendanceStatus)
        {
            context.AttendanceStatus.Add(s);
        }
    }

    public static void InitFeedbackType(DataContext context)
    {
        //Look for any attendance status
        if (context.FeedbackType.Any())
        {
            return;
        }

        var FeedbackType = new WebApi.Entities.FeedbackType[] {
            new WebApi.Entities.FeedbackType{id=1, name="Training Feedback"},
            new WebApi.Entities.FeedbackType{id=2, name="Attendance Feedback"},
            new WebApi.Entities.FeedbackType{id=3, name="User Feedback"},
            new WebApi.Entities.FeedbackType{id=4, name="Training Registration Feedback"}
        };

        foreach (var s in FeedbackType)
        {
            context.FeedbackType.Add(s);
        }
    }
}