using Domain.DTOs.Student;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IStudentService
{
    Task<Response<List<GetStudentDto>>> GetAllAsync(StudentFilter filter);
    Task<Response<GetStudentDto>> GetByIdAsync(int id);
    Task<Response<GetStudentDto>> AddAsync(CreateStudentDto createStudentDto);
    Task<Response<GetStudentDto>> UpDateAsync(int id, UpdateStudentDto updateStudentDto);
    Task<Response<string>> DeleteAsync(int id);

}
