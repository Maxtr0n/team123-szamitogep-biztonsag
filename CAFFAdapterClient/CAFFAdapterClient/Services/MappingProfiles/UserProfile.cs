using AutoMapper;
using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.Domain;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace CAFFAdapterClient.Services.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<JsonPatchDocument<EditUserDto>, JsonPatchDocument<User>>();
            CreateMap<Operation<EditUserDto>, Operation<User>>();
        }
    }
}
