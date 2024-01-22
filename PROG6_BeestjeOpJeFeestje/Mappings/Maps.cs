using AutoMapper;
using Domain.Models;
using PROG6_BeestjeOpJeFeestje.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace PROG6_BeestjeOpJeFeestje.Mappings;
[ExcludeFromCodeCoverage]

public class Maps : Profile
{
    public Maps()
    {
        CreateMap<User, UserPasswordVM>().ReverseMap();
        CreateMap<User, UserCreateVM>().ReverseMap();
        CreateMap<Animal, AnimalVM>().ReverseMap();
        CreateMap<Animal, AnimalCreateVM>().ReverseMap();

    }
    
}