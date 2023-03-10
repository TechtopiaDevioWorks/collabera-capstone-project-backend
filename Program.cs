using System.Text.Json.Serialization;
using WebApi.Helpers;
using WebApi.Services;
using Microsoft.AspNetCore.OData;
var builder = WebApplication.CreateBuilder(args);

{
    var services = builder.Services;
    var env = builder.Environment;
    var context = new DataContext(builder.Configuration);
    services.AddDbContext<DataContext>();
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        //x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }).AddOData(options => options.Select().Filter().OrderBy().SetMaxTop(null).Count());
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    services.AddScoped<IUserService, UserService>();
    services.AddScoped<ITeamService, TeamService>();
    services.AddScoped<IInviteService, InviteService>();
    services.AddScoped<ITrainingService, TrainingService>();
    services.AddScoped<ITrainingRegistrationService, TrainingRegistrationService>();
    services.AddScoped<IFeedbackService, FeedbackService>();
    services.AddScoped<IAttendanceService, AttendanceService>();
    DbInitializer.Initialize(context);
}


var app = builder.Build();


{
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseMiddleware<ErrorHandlerMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:4000");