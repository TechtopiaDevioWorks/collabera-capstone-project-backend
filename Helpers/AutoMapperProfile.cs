namespace WebApi.Helpers;

using AutoMapper;
using WebApi.Entities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // CreateRequest -> User
        CreateMap<WebApi.Models.User.CreateRequest, User>();

        // UpdateRequest -> User
        CreateMap<WebApi.Models.User.UpdateRequest, User>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;
                    if ((x.DestinationMember.Name == "role_id" || x.DestinationMember.Name == "team_id") && prop.Equals((byte)0)) return false;
                    // ignore null role
                    //if (x.DestinationMember.Name == "role" && src.role_id == 1) return false;

                    return true;
                }
            ));
        // CreateRequest -> Team
        CreateMap<WebApi.Models.Team.CreateRequest, Team>();

        // UpdateRequest -> Team
        CreateMap<WebApi.Models.Team.UpdateRequest, Team>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;
                    return true;
                }
            ));
        // CreateRequest -> Invite
        CreateMap<WebApi.Models.Invite.CreateRequest, Invite>();

        // CreateRequest -> Training
        CreateMap<WebApi.Models.Training.CreateRequest, Training>();

        // CreateRequest -> Training Registration
        CreateMap<WebApi.Models.TrainingRegistration.CreateRequest, TrainingRegistration>();

        // CreateRequest -> Attendance
        CreateMap<WebApi.Models.Attendance.CreateRequest, Attendance>();

        // CreateRequest -> Feedback
        CreateMap<WebApi.Models.Feedback.CreateRequest, Feedback>();
    }
}