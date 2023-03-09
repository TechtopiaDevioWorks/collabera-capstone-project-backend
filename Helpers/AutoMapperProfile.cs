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
    }
}