namespace WebApi.Helpers;

using AutoMapper;
using WebApi.Entities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // CreateRequest -> User
        CreateMap<WebApi.Models.Users.CreateRequest, User>();

        // UpdateRequest -> User
        CreateMap<WebApi.Models.Users.UpdateRequest, User>()
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
        CreateMap<WebApi.Models.Teams.CreateRequest, Team>();

        // UpdateRequest -> Team
        CreateMap<WebApi.Models.Teams.UpdateRequest, Team>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;
                    return true;
                }
            ));
    }
}