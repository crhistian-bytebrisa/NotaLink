using Mapster;
using NotaLink.Application.DTOs.Auth;
using NotaLink.Application.DTOs.Users;
using NotaLink.Domain.Entities;

namespace NotaLink.Application.Mapping
{
    public class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<User, UserDTO>.NewConfig();

            TypeAdapterConfig<RegisterDTO, User>.NewConfig();
                

        }
    }
}
