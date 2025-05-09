using AutoMapper;
using Domain.DTOs.Student;
using Domain.Entities;

namespace Infrastructure.AutoMapper;

public class InfrastructureProfile : Profile
{
    public InfrastructureProfile()
    {
        CreateMap<CreateStudentDto, Student>();
        CreateMap<Student, GetStudentDto>();
    }
}