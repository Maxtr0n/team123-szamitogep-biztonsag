using AutoMapper;
using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.Domain;
using Microsoft.AspNetCore.JsonPatch;

namespace CAFFAdapterClient.Services.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<JsonPatchDocument<EditUserDto>, JsonPatchDocument<User>>();
        }
    }
}
