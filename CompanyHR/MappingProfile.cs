using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyHR;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(c => c.FullAddress,
            opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

        CreateMap<Employee, EmployeeDto>();

        CreateMap<CompanyCreationDto, Company>();
        
        CreateMap<EmployeeCreationDto, Employee>();

        CreateMap<EmployeeUpdateDto, Employee>().ReverseMap();

        CreateMap<CompanyUpdateDto, Company>();

        CreateMap<UserRegistrationDto, User>();

    }
}